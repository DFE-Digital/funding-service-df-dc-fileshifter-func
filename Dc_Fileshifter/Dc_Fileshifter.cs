using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Dc_Fileshifter.Model;
using Dc_Fileshifter.service;
using doc_capture_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;

namespace Dc_Fileshifter
{
    public class Dc_Fileshifter
    {
        private readonly ISharePointProvider _provider;
        private readonly ILogger<Dc_Fileshifter> log;

        public Dc_Fileshifter(ISharePointProvider provider, ILogger<Dc_Fileshifter> _log)
        {
            _provider = provider;
            log = _log;
        }

        [Function("Dc_Fileshifter")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation($"Data received from HTTP request body: {requestBody}");

                FileDetail fileDetail = JsonConvert.DeserializeObject<FileDetail>(requestBody);
                log.LogInformation($"Retrieving Data from SQL DB for file id: {fileDetail.FileId}");
                DocData dcdata = await GetDCData(fileDetail.FileId.ToString());
                log.LogInformation($"Data received from SQL DB: \r\n{dcdata}");
                if (fileDetail == null || dcdata == null || dcdata.fileId == null)
                {
                    var response = req.CreateResponse(HttpStatusCode.BadRequest);
                    return response;
                }
                string blobContainer = "digital-forms-upload";
                string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                log.LogInformation($"processing for file : {dcdata.filePath}");
                BlobServiceClient blobServiceClient = new(storageConnectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainer);
                BlobClient blobClient = containerClient.GetBlobClient(dcdata.filePath);

                if (await blobClient.ExistsAsync())
                {
                    log.LogInformation($"Blob file exists at path : {dcdata.filePath}");
                    MemoryStream file = new();
                    var downloadResponse = await blobClient.DownloadToAsync(file);
                    file.Position = 0;
                    using var myBlob = new StreamReader(file);
                    log.LogInformation($"Blob file upload is in progress....");
                    var result = await _provider.UploadSingleFile(new FileUploadRequest
                    {
                        File = myBlob.BaseStream,
                        FileName = fileDetail.TargetFilePath,
                        Folder = fileDetail.TargetRootFolder
                    });
                    log.LogInformation($"File uploaded with response : {JsonConvert.SerializeObject(result)}");
                    log.LogInformation($"Successfully processed file : {fileDetail.TargetFilePath}");
                    {
                        var response = req.CreateResponse(HttpStatusCode.OK);
                        await response.WriteStringAsync("success");
                        return response;
                    }
                }
                else
                {
                    log.LogInformation($"Blob file does not exists at path : {dcdata.filePath}");
                    {
                        var response = req.CreateResponse(HttpStatusCode.NotFound);
                        await response.WriteStringAsync("Failure, blob file does not exist at the mentioned path.");
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message + ex.InnerException.Message + ex.InnerException.StackTrace}");
                {
                    var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                    await response.WriteStringAsync("An error occurred while processing the request.");
                    return response;
                }
            }

        }

        private async Task<DocData> GetDCData(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("DFSQLAPIKEY"));
                string Url = $"{Environment.GetEnvironmentVariable("DFSQLAPIURL")}/api/GetDocumentCapture/" + id;
                var response = await client.GetAsync(Url);
                response.EnsureSuccessStatusCode();
                var responseData = await response.Content.ReadAsStringAsync();
                var auditLog = JsonConvert.DeserializeObject<DocData>(responseData);
                return auditLog;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

