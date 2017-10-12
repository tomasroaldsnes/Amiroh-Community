using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Amiroh.Controllers
{
    class Controller
    {
        static CloudBlobContainer GetContainer(string containerType)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=amiroh;AccountKey=j8F4jJbnIjvgV1pRJXtc6TSb6AyAfOHa5Bu163iggCBGogRjLmRQRoAHrM4t2Q67FFlWAue/lfEtYQiT8C/ceA==");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerType);
            container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            
            return container;
        }

        public static async Task<Uri> UploadFileAsync(string containerType, Stream image)
        {
            CloudBlobContainer container = GetContainer(containerType);
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            fileBlob.Properties.ContentType = "image/jpg";
            
            await fileBlob.UploadFromStreamAsync(image);

            return fileBlob.Uri;


        }
        public async Task<byte[]> DownloadBytesAsync(Uri uri)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseContentRead);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        public static async Task PerformBlobOperation()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=amiroh;AccountKey=j8F4jJbnIjvgV1pRJXtc6TSb6AyAfOHa5Bu163iggCBGogRjLmRQRoAHrM4t2Q67FFlWAue/lfEtYQiT8C/ceA==");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");

            // Create the "myblob" blob with the text "Hello, world!"
            await blockBlob.UploadTextAsync("Hello, world!");
        }

        public static async Task<byte[]> GetFileAsync(string type, string name)
        {
            var container = GetContainer(type);

            var blob = container.GetBlobReference(name);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;
        }



    }
}

