using Microsoft.AspNetCore.Http;

namespace Sob_Need_Server.Services.Storage
{
    public interface IFileUploadService
    {
        /// <summary>
        /// Upload a file using the configured storage provider
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="options">Upload options</param>
        /// <returns>Upload result with file URL</returns>
        Task<FileUploadResult> UploadAsync(IFormFile file, FileUploadOptions options);
        
        /// <summary>
        /// Delete a file from storage
        /// </summary>
        /// <param name="fileUrl">URL or ID of the file to delete</param>
        /// <returns>True if successful</returns>
        Task<bool> DeleteAsync(string fileUrl);
        
        /// <summary>
        /// Validate file before upload
        /// </summary>
        /// <param name="file">File to validate</param>
        /// <param name="options">Validation options</param>
        /// <returns>Validation result</returns>
        FileValidationResult ValidateFile(IFormFile file, FileUploadOptions options);
    }
    
    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string FileUrl { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public long FileSize { get; set; }
        public string? FileId { get; set; } // Google Drive file ID
        public string? ErrorMessage { get; set; }
    }
    
    public class FileUploadOptions
    {
        public string[]? AllowedExtensions { get; set; }
        public long MaxFileSizeMB { get; set; } = 10;
        public string? Folder { get; set; }
    }
    
    public class FileValidationResult
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
