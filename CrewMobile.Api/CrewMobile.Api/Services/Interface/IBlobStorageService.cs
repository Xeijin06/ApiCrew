using Azure.Storage.Blobs.Models;

namespace CrewMobile.Api.Services.Interface
{
    public interface IBlobStorageService
    {
        Task<Uri> UploadAsync<T>(T content, string fileName, string? containerName = null, string? contentType = null, CancellationToken ct = default);
        Task DeleteOldFilesAsync(ILogStorageAccountService logStorageAccount, string? containerName = null, int? retentionDays = null, CancellationToken ct = default);
    }
}
