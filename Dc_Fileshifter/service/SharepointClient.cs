using Microsoft.Graph;
using Azure.Identity;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dc_Fileshifter.Configurations;
using Dc_Fileshifter.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dc_Fileshifter.service
{
    public class SharePointClient : ISharePointClient
    {
        private readonly ILogger<SharePointClient> _logger;
        private GraphServiceClient _graphClient;
        private readonly string _clientId = SharePointConfiguration.AppId;
        private readonly string _tenantId = SharePointConfiguration.TenantId;
        private readonly string _clientSecret = SharePointConfiguration.SecretKey;
        private readonly string _siteId = SharePointConfiguration.Sites; // Should be the site ID, not site name
        private readonly string _rootFolder = SharePointConfiguration.Site_RootFolder;

        public SharePointClient(ILogger<SharePointClient> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private void InitGraphClient()
        {
            var credential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
            _graphClient = new GraphServiceClient(credential);
        }

        public async Task<string> GetAccessToken()
        {
            InitGraphClient();
            return ""; // Not needed for Graph, kept for interface compatibility
        }

        public async Task<SharePointFileUploadResponse> UploadFile(FileUploadRequest request, string digestValue)
        {
            InitGraphClient();
            var fileName = Path.GetFileName(request.FileName);
            var folderPath = _rootFolder +"/"+ request.Folder;
            var siteId = _siteId;
            DriveItem driveItem = null;
            try
            {
               
                var drive = await _graphClient.Sites[siteId].Drive.GetAsync();
                var uploadPath = string.IsNullOrWhiteSpace(folderPath) ? fileName : $"{folderPath}/{fileName}";
                driveItem = await _graphClient.Drives[drive.Id].Root
                    .ItemWithPath(uploadPath)
                    .Content
                    .PutAsync(request.File);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error in Sharepoint Upload " + ex.Message + ex.StackTrace);
            }

            return new SharePointFileUploadResponse
            {
                FileName = driveItem.Name,
                FileUrl = driveItem.WebUrl
            };
        }

        public async Task CreateFolder(string folderName, string folderPath = null)
        {
            InitGraphClient();
            var siteId = _siteId;
            var drive = await _graphClient.Sites[siteId].Drive.GetAsync();
            var parentPath = string.IsNullOrWhiteSpace(folderPath) ? string.Empty : folderPath;
            await _graphClient.Drives[drive.Id].Root
                .ItemWithPath(parentPath)
                .Children
                .PostAsync(new DriveItem
                {
                    Name = folderName,
                    Folder = new Folder(),
                    AdditionalData = new Dictionary<string, object> { { "@microsoft.graph.conflictBehavior", "rename" } }
                });
        }

        public async Task<string> GetFolders(string folderName = null)
        {
            InitGraphClient();
            var siteId = _siteId;
            var drive = await _graphClient.Sites[siteId].Drive.GetAsync();
            var path = string.IsNullOrWhiteSpace(folderName) ? string.Empty : folderName;
            var folders = await _graphClient.Drives[drive.Id].Root
                .ItemWithPath(path)
                .Children
                .GetAsync();
            return JsonConvert.SerializeObject(folders.Value.Select(f => f.Name));
        }

        public async Task<SharePointContext> GetContextInfo()
        {
            // Not needed for Graph, kept for interface compatibility
            return new SharePointContext();
        }
    }
}

