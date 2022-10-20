//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;

//namespace LinBlobTest.Repositories
//{
//    public class AccessKeyBlobServiceClient : BlobServiceClient
//    {
//        private string _conn;
//        public AccessKeyBlobServiceClient(string connectionString)
//        {
//            _conn = connectionString;
//        } 
//    }

//    public class ImageRepository
//    {
//        private readonly BlobServiceClient _accessKeyBlobClient;

//        public ImageRepository(AccessKeyBlobServiceClient accessKeyBlobClient)
//        {
//            _accessKeyBlobClient = accessKeyBlobClient; 
//        }


//        public async Task<string> GetBlobContent(string containerName, string blobName)
//        {
//            var containerClient = new BlobContainerClient(_storageConnectionString, containerName);

//            BlobContainerClient containerClient = _accessKeyBlobClient.GetBlobContainerClient(containerName);
//            var blobClient = containerClient.GetBlobClient(blobName);
//            var blobDownload = await blobClient.DownloadContentAsync();
//            return blobDownload.Value.Content.ToString(); 
//        }

//    }
//}
