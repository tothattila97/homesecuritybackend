using HomeSecurity.Bll.Database;
using HomeSecurity.Bll.Database.Entities;
using HomeSecurity.Bll.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSecurity.Bll.Services
{
    public class UploadService
    {
        public HomeSecurityDbContext Context { get; }
        public UserManager<User> UserManager { get; }
        public IEmailService EmailService { get; }

        public UploadService(
            HomeSecurityDbContext context,
            UserManager<User> userManager,
            IEmailService emailService)
        {
            Context = context;
            UserManager = userManager;
            EmailService = emailService;
        }

        public async Task<bool> UploadImageToPersonalBlobStorage(Stream fileStream, string fileName, string azureConnection, int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(azureConnection.ToString());
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(user.ContainerId);
            await blobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(fileStream);

            EmailService.Send(new EmailMessageModel
            {
                Email = user.Email,
                Subject = "Upload notification",
                Content = "An image was uploaded to your storage. You be able to look them on our website!"
            });

            return await Task.FromResult(true);
        }

        public async Task<List<string>> GetThumbNailUrls(string accountName, string accountKey, string thumbnailContainer)
        {
            List<string> thumbnailUrls = new List<string>();

            // Create storagecredentials object by reading the values from the configuration (appsettings.json)
            StorageCredentials storageCredentials = new StorageCredentials(accountName, accountKey);

            // Create cloudstorage account by passing the storagecredentials
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

            // Create blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get reference to the container
            CloudBlobContainer container = blobClient.GetContainerReference(thumbnailContainer);

            BlobContinuationToken continuationToken = null;

            BlobResultSegment resultSegment = null;

            //Call ListBlobsSegmentedAsync and enumerate the result segment returned, while the continuation token is non-null.
            //When the continuation token is null, the last page has been returned and execution can exit the loop.
            do
            {
                //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                //or by calling a different overload.
                resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);

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
                        Permissions = SharedAccessBlobPermissions.Read
                    };

                    //Generate the shared access signature on the blob, setting the constraints directly on the signature.
                    string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

                    //Return the URI string for the container, including the SAS token.
                    thumbnailUrls.Add(blob.Uri + sasBlobToken);
                    
                }

                //Get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }

            while (continuationToken != null);

            return await Task.FromResult(thumbnailUrls);
        }

        public static bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}
