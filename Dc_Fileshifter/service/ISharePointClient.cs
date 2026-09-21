
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dc_Fileshifter.Model;
using Microsoft.AspNetCore.Http;

namespace Dc_Fileshifter.service
{
    public interface ISharePointClient
    {
        Task<SharePointFileUploadResponse> UploadFile(FileUploadRequest request, string digestValue);
        Task<SharePointContext> GetContextInfo();
        Task<string> GetAccessToken();
        Task<string> GetFolders(string folderName =null);
    }
}

