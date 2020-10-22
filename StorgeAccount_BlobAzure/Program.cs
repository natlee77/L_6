using StorageAccount_AzureBlob.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StorageAccount
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=blobazurewin20;AccountKey=YY53jnU4/8Lcr92qy0P3rcHW9X4XZllm7KGRy8eqHx44W6bqJs17Oovab3rr2Os1xaAndi7eGEqC+DcgIEiafQ==;EndpointSuffix=core.windows.net";
            var containerName = "blob";

            Console.WriteLine("Initializing Storage Account with containerName: " + containerName);
            await StorageService.InitializeStorageAsync(connectionString, containerName);//kan .Awaiter/Get.Awaiter/Task.Run



            var fileName = $"myfile-{Guid.NewGuid()}.txt";// ska ha unik namn
            var content = "This is the content of the file , variant";
            var filePath = Path.Combine(@"D:\Blob\", fileName);

            Console.WriteLine("Creating and writing content in file: " + filePath);
            await StorageService.WriteToFileAsync(filePath, content);


            Console.WriteLine("Uploading file to Azure Storage Blob in container: " + containerName);
            await StorageService.UploadFileAsync(filePath);




            var downloadPath = Path.Combine(@"D:\Blob\downloads\", fileName);

            Console.WriteLine("Downloading file from Azure Storage Blob to: " + Path.GetDirectoryName(downloadPath));
            await StorageService.DownloadFileAsync(downloadPath);

            //read download  file
            Console.WriteLine("Reading content from file: " + downloadPath);
            Console.WriteLine(await StorageService.ReadDownloadedFileAsync(downloadPath));


            //    //ladda bild till Azure
            //    var fileName = $"s.jpg";
            //    var filePath = Path.Combine(@"D:\Blob\", fileName);
            //    Console.WriteLine("Initializing Storage Account with containerName: " + containerName);
            //    await StorageService.InitializeStorageAsync(connectionString, containerName);
            //    Console.WriteLine("Uploading file to Azure Storage Blob in container: " + containerName);
            //    await StorageService.UploadFileAsync(@"D:\Blob\s.jpg");

        }
    }
}
