using Microsoft.AspNetCore.Http;

namespace ERPSystem.Application.Interfaces;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string subFolder);
    void DeleteFile(string filePath);
}
