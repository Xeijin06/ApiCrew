using Azure.Storage.Blobs;
using CrewMobile.Api.Models;
using CrewMobile.Api.Services.Interface;
using CrewMobile.Common.Models;
using CrewMobile.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CrewMobileApi.Business
{
    public class ProcessFile
    {
        #region Attributes
        /// <summary>
        /// The data base context
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// The Blob Storage Options
        /// </summary>
        private readonly AzureStorageOptions _storageOptions;

        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString;

        #endregion

        #region Constructors
        public ProcessFile(ApplicationDbContext db, AzureStorageOptions options, string connectionString)
        {
            this.db = db;
            _storageOptions = options;
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        #endregion

        public async Task<int> Process(ILogStorageAccountService logStorageAccount, string fileName, CancellationToken ct = default)
        {
            int counter = 0;
            string? line;

            try
            {

                var blobClient = new BlobClient(_storageOptions.ConnectionString, _storageOptions.DefaultContainer, fileName);

                if (!await blobClient.ExistsAsync(ct).ConfigureAwait(false))
                    throw new FileNotFoundException($"No se encontró el blob storage: {fileName}");

                await DeleteOldRecords().ConfigureAwait(false);

                var flightCrewsToAdd = new List<FlightCrewCom>(5000);

                await using (var downloadStream = await blobClient.OpenReadAsync(cancellationToken: ct).ConfigureAwait(false))
                using (var reader = new StreamReader(downloadStream))
                {
                    var buffer = new List<string>(5000);

                    while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                    {
                        buffer.Add(line);
                        counter++;

                        if (counter % 100 == 0)
                        {
                            logStorageAccount.Log(CrewMobile.Api.Models.LogLevel.Information,  $"Processed {counter}...");
                        }

                        if (buffer.Count >= 5000)
                        {
                            await ProcessBatch(buffer, flightCrewsToAdd).ConfigureAwait(false);
                            buffer.Clear();

                            if (flightCrewsToAdd.Count >= 5000)
                            {
                                await BulkInsertFlightCrewsAsync(flightCrewsToAdd).ConfigureAwait(false);
                                flightCrewsToAdd.Clear();
                            }
                        }
                    }

                    if (buffer.Count > 0)
                    {
                        await ProcessBatch(buffer, flightCrewsToAdd).ConfigureAwait(false);
                        buffer.Clear();
                    }

                    if (flightCrewsToAdd.Count > 0)
                    {
                        await BulkInsertFlightCrewsAsync(flightCrewsToAdd).ConfigureAwait(false);
                        flightCrewsToAdd.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                await SaveLog($"Error: {ex.Message}", false).ConfigureAwait(false);
            }

            return counter;
        }

        private async Task ProcessBatch(List<string> buffer, List<FlightCrewCom> flightCrewsToAdd)
        {
            foreach (var line in buffer)
            {
                if (line.Length > 61)
                {
                    var objFlight = DeserializationList(line);

                    flightCrewsToAdd.AddRange(objFlight);

                    if (flightCrewsToAdd.Count >= 5000) // Guardar en lotes más grandes
                    {
                        await BulkInsertFlightCrewsAsync(flightCrewsToAdd);
                        flightCrewsToAdd.Clear();
                    }
                }
            }
        }

        public DataTable ToDataTable(List<FlightCrewCom> crews)
        {
            var table = new DataTable();
            table.Columns.Add("FlightNumber", typeof(int));
            table.Columns.Add("Company", typeof(string));
            table.Columns.Add("DateStart", typeof(DateTime));
            table.Columns.Add("DateEnd", typeof(DateTime));
            table.Columns.Add("Source", typeof(string));
            table.Columns.Add("Destination", typeof(string));
            table.Columns.Add("CrewRoll", typeof(string));
            table.Columns.Add("CrewId", typeof(int));
            table.Columns.Add("CrewName", typeof(string));

            foreach (var crew in crews)
            {
                table.Rows.Add(
                    crew.FlightNumber,
                    crew.Company,
                    crew.DateStart,
                    crew.DateEnd,
                    crew.Source,
                    crew.Destination,
                    crew.CrewRoll,
                    crew.CrewId,
                    crew.CrewName
                );
            }
            return table;
        }

        public async Task BulkInsertFlightCrewsAsync(List<FlightCrewCom> crews)
        {
            var dataTable = ToDataTable(crews);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "FlightCrews"; // Nombre de tu tabla en SQL Server

                    // Mapea las columnas si los nombres no coinciden exactamente
                    bulkCopy.ColumnMappings.Add("FlightNumber", "FlightNumber");
                    bulkCopy.ColumnMappings.Add("Company", "Company");
                    bulkCopy.ColumnMappings.Add("DateStart", "DateStart");
                    bulkCopy.ColumnMappings.Add("DateEnd", "DateEnd");
                    bulkCopy.ColumnMappings.Add("Source", "Source");
                    bulkCopy.ColumnMappings.Add("Destination", "Destination");
                    bulkCopy.ColumnMappings.Add("CrewRoll", "CrewRoll");
                    bulkCopy.ColumnMappings.Add("CrewId", "CrewId");
                    bulkCopy.ColumnMappings.Add("CrewName", "CrewName");

                    await bulkCopy.WriteToServerAsync(dataTable);
                }
            }
        }


        /// <summary>
        /// Delete old records
        /// </summary>
        private async Task DeleteOldRecords()
        {
            await db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE FlightCrews");
        }

        /// <summary>
        /// Delete all rows if has pre existence of my flight in my database
        /// </summary>
        /// <param name="fCrew"></param>
        private void DeletePreviousData(FlightCrew fCrew)
        {
            // var baseQuery = from ep in db.FlightCrews where 
            var fCrews = db.FlightCrews
                           .Where(f => f.FlightNumber == fCrew.FlightNumber && f.DateStart == fCrew.DateStart);
            db.FlightCrews.RemoveRange(fCrews);
            db.SaveChanges();
        }

        /// <summary>
        /// Create objects from a readed line from the file generated for AIMS
        /// </summary>
        /// <param name="line"></param>
        /// <returns>IEnumerable FlightCrew </returns>

        private List<FlightCrew> DeserializationList(string line)
        {
            List<FlightCrew> listFly = new List<FlightCrew>();

            try
            {
                int temp = 0;
                FlightCrew obj = new FlightCrew();
                obj.FlightNumber = int.Parse(line.Substring(temp, FileModel.IdLength).Trim());
                temp += FileModel.IdLength;
                obj.Company = line.Substring(temp, FileModel.CompanyLength).Trim();
                temp += FileModel.CompanyLength;
                obj.DateStart = DateTime.ParseExact(
                    line.Substring(temp, FileModel.DateLength).Trim(), "ddMMyyyy",
                    System.Globalization.CultureInfo.InvariantCulture
                    );
                temp += FileModel.DateLength;
                obj.Source = line.Substring(temp, FileModel.OriginLength).Trim();
                temp += FileModel.OriginLength;
                obj.Destination = line.Substring(temp, FileModel.DestinationLength).Trim();

                var hourStartString = obj.Destination.Substring(4, 6).Trim();
                var hourStartS = hourStartString.Substring(0, 2);
                var hourStart = int.Parse(hourStartS);
                var minuteStartS = hourStartString.Substring(3, 2);
                var minuteStart = int.Parse(minuteStartS);

                var hourEndString = obj.Destination.Substring(10, 6).Trim();
                var hourEndS = hourEndString.Substring(0, 2);
                var hourEnd = int.Parse(hourEndS);
                var minuteEndS = hourEndString.Substring(3, 2);
                var minuteEnd = int.Parse(minuteEndS);

                var dateStart = obj.DateStart;
                var dateEnd = obj.DateStart;

                dateStart = dateStart.AddHours(hourStart);
                dateStart = dateStart.AddMinutes(minuteStart);
                dateEnd = dateEnd.AddHours(hourEnd);
                dateEnd = dateEnd.AddMinutes(minuteEnd);

                if (hourStart * 60 + minuteStart > hourEnd * 60 + minuteEnd)
                {
                    dateEnd = dateEnd.AddDays(1);
                }

                obj.Destination = obj.Destination.Substring(0, 3);
                temp += FileModel.DestinationLength;

                while (temp < line.Length)
                {
                    obj.CrewRoll = line.Substring(temp, FileModel.JobLength).Trim();
                    temp += FileModel.JobLength;
                    obj.CrewId = int.Parse(line.Substring(temp, FileModel.EmpIdLength).Trim());
                    temp += FileModel.EmpIdLength;
                    obj.CrewName = line.Substring(temp, FileModel.NameLength).Trim();
                    temp += FileModel.NameLength;
                    listFly.Add(new FlightCrew
                    {
                        FlightNumber = obj.FlightNumber,
                        Company = obj.Company,
                        DateStart = dateStart,
                        DateEnd = dateEnd,
                        Source = obj.Source,
                        Destination = obj.Destination,
                        CrewRoll = obj.CrewRoll,
                        CrewName = "", //obj.CrewName,
                        CrewId = obj.CrewId
                    });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return listFly;
        }

        //TODO: Validar si este metodo se puede mover a una clase comun o a un helper
        private async Task SaveLog(string message, bool wasSucces)
        {
            var proccessFileLog2 = new ProccessFileLog
            {
                Date = DateTime.Now,
                Steps = message,
                WasSuccess = wasSucces,
            };

            db.ProccessFileLogs.Add(proccessFileLog2);
            await db.SaveChangesAsync();
        }
    }
}
