using ERPSystem.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ERPSystem.Application.Services;

public class FileService(IWebHostEnvironment env) : IFileService
{
    public async Task<string> UploadFileAsync(IFormFile file, string subFolder)
    {
        if (file == null || file.Length == 0)
            return string.Empty;

        var uploadsFolder = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", subFolder);
        
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/uploads/{subFolder}/{uniqueFileName}";
    }

    public void DeleteFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;
        
        var physicalPath = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), filePath.TrimStart('/'));

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
    }
}
