using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LinBlobTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LinBlobTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessKey([FromQuery] string b)
        {
            var model = new ImageModel { BlobName = "accesskey" };
            return View(model);
        } 

        [HttpPost]
        public   IActionResult  AccessKey(ImageModel model)
        {
            string containerName = _configuration.GetValue<string>("ContainerName");
            string storageConnectionString = _configuration.GetConnectionString("StorageConnectionString");

            BlobContainerClient containerClient = new BlobContainerClient(storageConnectionString, containerName);
            BlobClient blobClient = containerClient.GetBlobClient(model.BlobName);
            model.ImageBytes = GetBlobDataBytes(  blobClient).GetAwaiter().GetResult();
            return View(model);
        }

        private async Task<byte[]> GetBlobDataBytes( BlobClient blobClient)
        {
            byte[] result = Array.Empty<byte>();
            if (await blobClient.ExistsAsync())
            {
                BlobDownloadInfo download = await blobClient.DownloadAsync();
                result = new byte[download.ContentLength];
                await download.Content.ReadAsync(result, 0, (int)download.ContentLength); 
            }
            return result;
        }

        public IActionResult ManagedIdentity( )
        {
            var model = new ImageModel { BlobName = "managedidentity" };
            return View(model);
        }

        [HttpPost]
        public IActionResult ManagedIdentity(ImageModel model)
        { 
            //string accountName = "linblobtest2022";
            //string storageUri = $"https://{accountName}.blob.core.windows.net"; 

            // install Azure.Identity
            string containerName = _configuration.GetValue<string>("ContainerName");
            TokenCredential credential = new DefaultAzureCredential();
            var storageUri = _configuration.GetValue<string>("StorageUri");

            string uri = $@"{storageUri}/{containerName}/{model.BlobName}"; 
            BlobClient blobClient = new BlobClient(new Uri(uri), credential);
            model.ImageBytes = GetBlobDataBytes(  blobClient).GetAwaiter().GetResult();
            return View(model); 
        }
        public IActionResult UserManagedIdentity()
        {
            var model = new ImageModel { BlobName = "usermanagedidentity" };
            return View(model);
        }
        [HttpPost]
        public IActionResult UserManagedIdentity(ImageModel model)
        {
            string managedIdentityClientId = _configuration.GetValue<string>("UserManagedIdentityClientId");        
            string containerName = _configuration.GetValue<string>("ContainerName"); 
            var storageUri = _configuration.GetValue<string>("StorageUri");

            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityClientId });

            string uri = $@"{storageUri}/{containerName}/{model.BlobName}";
            BlobClient blobClient = new BlobClient(new Uri(uri), credential);
            model.ImageBytes = GetBlobDataBytes(blobClient).GetAwaiter().GetResult();
            return View(model);
        }
     
        public IActionResult ApplicationObject( )
        {
            var model = new ImageModel { BlobName = "bb8" };
            return View(model);
        }

        [HttpPost]
        public IActionResult ApplicationObject(ImageModel model)
        {
            string containerName =  _configuration.GetValue<string>("ContainerName");
            string tenantId = _configuration.GetValue<string>("TenantId");
            string clientId = _configuration.GetValue<string>("ClientId"); 
            string clientSecret = _configuration.GetValue<string>("ClientSecret");

            // Install Azure.Identity
            ClientSecretCredential clientCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var storageUri = _configuration.GetValue<string>("StorageUri");
            string uri = $@"{storageUri}/{containerName}/{model.BlobName}";

            BlobClient blobClient = new BlobClient(new Uri(uri), clientCredential); 
            model.ImageBytes = GetBlobDataBytes( blobClient).GetAwaiter().GetResult();
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}