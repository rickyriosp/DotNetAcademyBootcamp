namespace GameStore.Api.Shared.FileUpload;

public class FileUploader(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
{
    public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder)
    {
        var result = new FileUploadResult();

        if (file == null || file.Length == 0)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "File is empty.";
            return result;
        }

        if (file.Length > 10 * 1024 * 1024) // 10 MB limit
        {
            result.IsSuccess = false;
            result.ErrorMessage = "File size exceeds the 10 MB limit.";
            return result;
        }

        var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(fileExtension) || !permittedExtensions.Contains(fileExtension))
        {
            result.IsSuccess = false;
            result.ErrorMessage = "Invalid file type. Only image files are allowed.";
            return result;
        }

        var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadFolder, uniqueFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var httpContext = httpContextAccessor.HttpContext;
        var fileUrl = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}/{folder}/{uniqueFileName}";

        result.IsSuccess = true;
        result.FileUrl = fileUrl;
        return result;
    }
}