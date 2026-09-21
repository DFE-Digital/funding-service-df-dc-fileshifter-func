using System;
using System.Threading.Tasks;
using Dc_Fileshifter.Configurations;
using Dc_Fileshifter.Model;
using Microsoft.Extensions.Logging;

namespace Dc_Fileshifter.service
{
    public class SharePointProvider : ISharePointProvider
    {
        private readonly ILogger<SharePointProvider> _logger;
        private readonly ISharePointClient _sharePointClient;

        public SharePointProvider(ILogger<SharePointProvider> logger, ISharePointClient sharePointClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sharePointClient = sharePointClient ?? throw new ArgumentNullException(nameof(sharePointClient));
            
        }

        public async Task<SharePointFileUploadResponse> UploadSingleFile(FileUploadRequest request)
        {
            await _sharePointClient.GetAccessToken();

            var context = await _sharePointClient.GetContextInfo();


            var uploadFileResponse = await _sharePointClient.UploadFile(request, context.Response?.GetContextWebInformation?.FormDigestValue);

            if (uploadFileResponse == null)
            {
                _logger.LogError("SharePointProvider.UploadSingleFile SharePointClient.UploadFile returns null");
                throw new SystemException("Error in upload single");
            }

            return uploadFileResponse;
        }



    }
}

