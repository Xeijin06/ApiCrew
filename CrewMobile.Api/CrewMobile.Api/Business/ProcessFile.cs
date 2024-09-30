using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrewMobile.Api.Models;
using CrewMobile.Common.Models;
using CrewMobile.Domain.Models;

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
        /// The stream writer in file log for FTP process
        /// </summary>
        private StreamWriter streamWriter;
        
        /// <summary>
        /// File Path for FTP process
        /// </summary>
        private string Path { get; set; }
        #endregion

        #region Constructors
        public ProcessFile(string path, ApplicationDbContext db)
        {
            this.db = db;
            Path = path;
        }
        #endregion

        public async Task<int> Process(StreamWriter streamWriter)
        {
            this.streamWriter = streamWriter;
            int counter = 0;
            string line;

            try
            {
                if (!File.Exists(Path))
                {
                    throw new FileNotFoundException();
                }

                await DeleteOldRecords();

                StreamReader file = new StreamReader(Path);

                while ((line = file.ReadLine()) != null)
                {
                    counter++;

                    if (counter % 100 == 0)
                    {
                        this.streamWriter.WriteLine(string.Format("{0} - Proceced {1}...", DateTime.Now, counter));
                        this.streamWriter.Flush();
                    }

                    if (line.Length > 61)//validate if has crew
                    {
                        List<FlightCrew> objFlight = DeserializationList(line);
                        DeletePreviousData(objFlight[0]);
                        foreach (var item in objFlight)
                        {
                            db.FlightCrews.Add(new FlightCrew
                            {
                                Company = item.Company,
                                CrewId = item.FlightCrewId,
                                CrewName = item.CrewName,
                                CrewRoll = item.CrewRoll,
                                DateStart = item.DateStart,
                                DateEnd = item.DateEnd,
                                Destination = item.Destination,
                                FlightCrewId = item.FlightCrewId,
                                FlightNumber = item.FlightNumber,
                                Source = item.Source,
                            });
                        }
                    }
                }

                file.Close();
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await SaveLog(string.Format("Error: {0}", ex.Message), false);
            }

            return counter;
        }

        /// <summary>
        /// Delete old records
        /// </summary>
        private async Task DeleteOldRecords()
        {
            var date = DateTime.Today.AddHours(-5);
            var oldRecords = db.FlightCrews.Where(fc => fc.DateStart < date);
            db.FlightCrews.RemoveRange(oldRecords);
            await db.SaveChangesAsync();
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
                    obj.FlightCrewId = int.Parse(line.Substring(temp, FileModel.EmpIdLength).Trim());
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
                        FlightCrewId = obj.FlightCrewId,
                        CrewName = obj.CrewName
                    });
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return listFly;
        }

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
