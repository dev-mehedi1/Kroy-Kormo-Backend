using Microsoft.AspNetCore.Http;

namespace Sob_Need_Server.Services.Storage
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IGoogleDriveService _googleDriveService;
        
        public FileUploadService(IGoogleDriveService googleDriveService)
        {
            _googleDriveService = googleDriveService;
        }
        
        public async Task<FileUploadResult> UploadAsync(IFormFile file, FileUploadOptions options)
        {
            // Validate file first
            var validation = ValidateFile(file, options);
            if (!validation.IsValid)
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = validation.ErrorMessage,
                    FileName = file.FileName,
                    FileSize = file.Length
                };
            }
            
            try
            {
                using var stream = file.OpenReadStream();
                var fileId = await _googleDriveService.UploadFileAsync(stream, file.FileName, file.ContentType);
                var fileUrl = await _googleDriveService.GetFileUrlAsync(fileId);
                
                return new FileUploadResult
                {
                    Success = true,
                    FileUrl = fileUrl,
                    FileName = file.FileName,
                    FileSize = file.Length,
                    FileId = fileId
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = $"Upload failed: {ex.Message}",
                    FileName = file.FileName,
                    FileSize = file.Length
                };
            }
        }
        
        public async Task<bool> DeleteAsync(string fileUrl)
        {
            try
            {
                // Extract file ID from URL or use directly if it's an ID
                var fileId = ExtractFileIdFromUrl(fileUrl);
                return await _googleDriveService.DeleteFileAsync(fileId);
            }
            catch
            {
                return false;
            }
        }
        
        public FileValidationResult ValidateFile(IFormFile file, FileUploadOptions options)
        {
            if (file == null || file.Length == 0)
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "File is empty or not provided"
                };
            }
            
            // Check file size
            var maxSizeBytes = options.MaxFileSizeMB * 1024 * 1024;
            if (file.Length > maxSizeBytes)
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"File size exceeds maximum allowed size of {options.MaxFileSizeMB}MB"
                };
            }
            
            // Check file extension
            if (options.AllowedExtensions != null && options.AllowedExtensions.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
                if (!options.AllowedExtensions.Contains(extension))
                {
                    return new FileValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = $"File type '.{extension}' is not allowed. Allowed types: {string.Join(", ", options.AllowedExtensions)}"
                    };
                }
            }
            
            return new FileValidationResult { IsValid = true };
        }
        
        private string ExtractFileIdFromUrl(string fileUrl)
        {
            // If it's already a file ID (no slashes), return as is
            if (!fileUrl.Contains("/") && !fileUrl.Contains("http"))
            {
                return fileUrl;
            }
            
            // Extract from Google Drive URL patterns
            // https://drive.google.com/file/d/{fileId}/view
            // https://drive.google.com/uc?id={fileId}
            if (fileUrl.Contains("drive.google.com"))
            {
                var patterns = new[]
                {
                    @"/d/([^/]+)",
                    @"id=([^&]+)"
                };
                
                foreach (var pattern in patterns)
                {
                    var match = System.Text.RegularExpressions.Regex.Match(fileUrl, pattern);
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }
            }
            
            // If we can't extract, assume it's the file ID
            return fileUrl;
        }
    }
}
