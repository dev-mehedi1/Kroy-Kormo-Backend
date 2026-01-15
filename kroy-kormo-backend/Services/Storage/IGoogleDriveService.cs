using Microsoft.AspNetCore.Http;

namespace Sob_Need_Server.Services.Storage
{
    public interface IGoogleDriveService
    {
        /// <summary>
        /// Upload a file to Google Drive
        /// </summary>
        /// <param name="fileStream">File stream to upload</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="mimeType">MIME type of the file</param>
        /// <returns>File ID from Google Drive</returns>
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string mimeType);
        
        /// <summary>
        /// Delete a file from Google Drive
        /// </summary>
        /// <param name="fileId">Google Drive file ID</param>
        /// <returns>True if successful</returns>
        Task<bool> DeleteFileAsync(string fileId);
        
        /// <summary>
        /// Get shareable/downloadable URL for a file
        /// </summary>
        /// <param name="fileId">Google Drive file ID</param>
        /// <returns>Public URL to access the file</returns>
        Task<string> GetFileUrlAsync(string fileId);
    }
}
