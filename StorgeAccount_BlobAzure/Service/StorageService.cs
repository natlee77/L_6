using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccount_AzureBlob.Services
{
    public static class StorageService  //ska ha olika tjänster 
    {
        private static BlobContainerClient _containerClient { get; set; }
        private static BlobClient _blobClient { get; set; }



        public static async Task InitializeStorageAsync(string connectionString, string containerName, bool uniqueName = false)
        {
            if (uniqueName)
                containerName = $"{containerName}-{Guid.NewGuid()}"; // GUID skapar unik namn till container- varja gång vi generera

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                try  // försök skapa container , onm det redan finns en
                {
                    _containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
                }
                catch //då
                {
                    try //hämta container som existeras
                    {
                        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    }
                    catch { } // kan ha mdl-- nån gick fel
                }
            }
            catch { }
        }  //++ i programm

        public static async Task WriteToFileAsync(string @filePath, string content)// skapa fil med namn , och write in file
        {   // @= D:\Blob\download\
            //  =D:\\Blob\\download\\

            try // måste ha file directory -wheare file ska ligga
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))// OM DET FINNS INTE D:\Blob\download
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)); //DÅ SKAPA DIRECTORY TILL

                await File.WriteAllTextAsync(filePath, content);
            }
            catch { }
        }// ++I programm

        public static async Task UploadFileAsync(string @filePath)
        {
            try
            {
                _blobClient = _containerClient.GetBlobClient(Path.GetFileName(filePath));

                using FileStream fileStream = File.OpenRead(filePath);//oppna file/ tar innerhållet  och skapa new fil

                await _blobClient.UploadAsync(fileStream, true);//ladda up file/cancelation token- true
                fileStream.Close();

                File.Delete(filePath);
            }
            catch { }
        }//ladda upp fil till azure


        public static async Task DownloadFileAsync(string @downloadPath)//från azure
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(downloadPath)))//hitta/hämta directory , om!
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));//då skapa

                BlobDownloadInfo download = await _blobClient.DownloadAsync();

                using FileStream fileStream = File.OpenWrite(downloadPath);//hämta/oppna fil och skriva till (downloadPath)
                await download.Content.CopyToAsync(fileStream);//ladda ner och copiera innerhålet till varan filestream

                fileStream.Close();
            }
            catch { }
            // Console.WriteLine(await File.ReadAllTextAsync(downloadPath));//flyttad till egen f.
        }

        public static async Task<string> ReadDownloadedFileAsync(string @downloadPath)
        {
            try
            {
                //Console.WriteLine(await File.ReadAllTextAsync(downloadPath));
                return await File.ReadAllTextAsync(downloadPath);
            }
            catch { }

            return "";
        }
    }
}