using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSecurity.Bll.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HomeSecurity.Web.Pages.Azure
{
    [Authorize]
    public class UploadedImagesModel : PageModel
    {
        public IConfiguration Configuration { get; }
        public UserManager<User> UserManager { get; }
        public string AccountName { get; }
        public string AccountKey { get; }

        [BindProperty]
        public ICollection<ImageModel> ContainerImageUrls { get; set; } = new List<ImageModel>();

        public UploadedImagesModel(IConfiguration configuration,
            UserManager<User> userManager)
        {
            Configuration = configuration;
            AccountName = Configuration.GetSection("AzureStorageConfig").GetValue<string>("AccountName");
            AccountKey = Configuration.GetSection("AzureStorageConfig").GetValue<string>("AccountKey");
            UserManager = userManager;
        }

        public class ImageModel
        {
            public string Uri { get; set; }
            public DateTimeOffset? CreationTime { get; set; }
            public DateTimeOffset? ModifiedTime { get; set; }
            public long Length { get; set; }

        }

        public void OnGet()
        {
            // Create storagecredentials object by reading the values from the configuration (appsettings.json)
            StorageCredentials storageCredentials = new StorageCredentials(AccountName, AccountKey);

            // Create cloudstorage account by passing the storagecredentials
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

            // Create blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get reference to the container
            CloudBlobContainer container = blobClient.GetContainerReference(UserManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult().ContainerId);

            BlobContinuationToken continuationToken = null;

            BlobResultSegment resultSegment = null;

            do
            {
                //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                //or by calling a different overload.
                resultSegment = container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null).GetAwaiter().GetResult();

                foreach (var blobItem in resultSegment.Results)
                {
                    CloudBlockBlob blob = blobItem as CloudBlockBlob;
                    //Set the expiry time and permissions for the blob.
                    //In this case, the start time is specified as a few minutes in the past, to mitigate clock skew.
                    //The shared access signature will be valid immediately.
                    SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy
                    {
                        SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                        SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24),
                        Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Delete
                    };

                    //Generate the shared access signature on the blob, setting the constraints directly on the signature.
                    string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

                    //Return the URI string for the container, including the SAS token.
                    //thumbnailUrls.Add(blob.Uri + sasBlobToken);
                    ContainerImageUrls.Add(new ImageModel
                    {
                        Uri = blob.Uri + sasBlobToken,
                        CreationTime = blob.Properties.Created,
                        ModifiedTime = blob.Properties.LastModified,
                        Length = blob.Properties.Length
                    });
                }

                //Get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }

            while (continuationToken != null);

            //return await Task.FromResult(ContainerImageUrls);
        }
    }
}