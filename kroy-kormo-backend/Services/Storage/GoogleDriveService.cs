using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.Extensions.Configuration;

namespace Sob_Need_Server.Services.Storage
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly string _folderId;
        
        public GoogleDriveService(IConfiguration configuration)
        {
            var credentialsPath = configuration["GoogleDrive:CredentialsPath"];
            _folderId = configuration["GoogleDrive:FolderId"] ?? throw new InvalidOperationException("Google Drive FolderId not configured");
            
            if (string.IsNullOrEmpty(credentialsPath) || !File.Exists(credentialsPath))
            {
                throw new InvalidOperationException($"Google Drive credentials file not found at: {credentialsPath}");
            }

            GoogleCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(DriveService.ScopeConstants.Drive);
            }
            
            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Kroy-Kormo Job Portal"
            });
        }
        
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string mimeType)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File
                {
                    Name = fileName,
                    Parents = new List<string> { _folderId }
                };
                
                var request = _driveService.Files.Create(fileMetadata, fileStream, mimeType);
                request.Fields = "id, webViewLink, webContentLink";
                
                var uploadProgress = await request.UploadAsync();
                
                if (uploadProgress.Status != UploadStatus.Completed)
                {
                    throw new Exception($"Upload failed: {uploadProgress.Exception?.Message}");
                }
                
                var file = request.ResponseBody;
                
                // Make file publicly accessible
                var permission = new Google.Apis.Drive.v3.Data.Permission
                {
                    Type = "anyone",
                    Role = "reader"
                };
                
                await _driveService.Permissions.Create(permission, file.Id).ExecuteAsync();
                
                return file.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload file to Google Drive: {ex.Message}", ex);
            }
        }
        
        public async Task<bool> DeleteFileAsync(string fileId)
        {
            try
            {
                await _driveService.Files.Delete(fileId).ExecuteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<string> GetFileUrlAsync(string fileId)
        {
            try
            {
                var request = _driveService.Files.Get(fileId);
                request.Fields = "webViewLink, webContentLink";
                var file = await request.ExecuteAsync();
                
                // Return direct download link
                return file.WebContentLink ?? file.WebViewLink ?? $"https://drive.google.com/file/d/{fileId}/view";
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get file URL: {ex.Message}", ex);
            }
        }
    }
}
