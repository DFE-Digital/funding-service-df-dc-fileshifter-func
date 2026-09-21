using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dc_Fileshifter.Model;
using Microsoft.AspNetCore.Http;

namespace Dc_Fileshifter.service
{
    public interface ISharePointProvider
    {
        Task<SharePointFileUploadResponse> UploadSingleFile(FileUploadRequest request);
        

    }
}

