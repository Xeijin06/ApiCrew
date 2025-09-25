using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CrewMobile.Api.Services.Interface;
using CrewMobile.Common.Models;
using Microsoft.Extensions.Options;
using System.Text;

namespace CrewMobile.Api.Services
{
    public sealed class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly AzureStorageOptions _options;

        public BlobStorageService(IOptions<AzureStorageOptions> options)
        {
            _options = options.Value;
            _blobServiceClient = new BlobServiceClient(_options.ConnectionString);
        }

        public async Task<Uri> UploadAsync<T>(
            T content,
            string fileName,
            string? containerName = null,
            string? contentType = null,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName es obligatorio.", nameof(fileName));

            string targetContainer = string.IsNullOrWhiteSpace(containerName) ? _options.DefaultContainer : containerName;

            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(targetContainer);
            await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

            BlobClient blob = container.GetBlobClient(fileName);

            // Normaliza el contenido genérico T a Stream
            using Stream dataStream = ToStream(content);
            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = string.IsNullOrWhiteSpace(contentType) ? DetectContentType(fileName) : contentType
            };

            // Sobrescribe si existe
            await blob.UploadAsync(dataStream, new BlobUploadOptions
            {
                HttpHeaders = httpHeaders,
                TransferOptions = new StorageTransferOptions
                {
                    MaximumConcurrency = Environment.ProcessorCount
                }
            }, ct);

            return blob.Uri;
        }

        private static Stream ToStream<T>(T content)
        {
            switch (content)
            {
                case Stream s:
                    // Garantiza posición al inicio si el stream es seekable
                    if (s.CanSeek) s.Position = 0;
                    return s;

                case byte[] bytes:
                    return new MemoryStream(bytes, writable: false);

                case ReadOnlyMemory<byte> rom:
                    return new MemoryStream(rom.ToArray(), writable: false);

                case string str:
                    // Si parece Base64, intenta decodificar; si no, sube como UTF-8
                    if (LooksLikeBase64(str))
                    {
                        byte[] decoded = Convert.FromBase64String(str);
                        return new MemoryStream(decoded, writable: false);
                    }
                    return new MemoryStream(Encoding.UTF8.GetBytes(str), writable: false);

                default:
                    // Serializa a JSON por defecto para tipos arbitrarios
                    string json = System.Text.Json.JsonSerializer.Serialize(content);
                    return new MemoryStream(Encoding.UTF8.GetBytes(json), writable: false);
            }
        }

        private static bool LooksLikeBase64(string value)
        {
            // Heurística ligera: longitud múltiplo de 4 y caracteres válidos
            if (string.IsNullOrWhiteSpace(value) || value.Length % 4 != 0) return false;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                bool ok =
                    (c >= 'A' && c <= 'Z') ||
                    (c >= 'a' && c <= 'z') ||
                    (c >= '0' && c <= '9') ||
                    c == '+' || c == '/' || c == '=';
                if (!ok) return false;
            }
            return true;
        }

        private static string DetectContentType(string fileName)
        {
            // Mapeo mínimo; amplía según tus necesidades o usa un paquete de MIME types
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain; charset=utf-8",
                ".json" => "application/json; charset=utf-8",
                ".csv" => "text/csv; charset=utf-8",
                ".xml" => "application/xml; charset=utf-8",
                _ => "application/octet-stream"
            };
        }
    }
}
