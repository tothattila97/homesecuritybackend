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

        public async Task<bool> UploadImageToPersonalBlobStorage(Stream fileStream, string fileName, string azureConnection, int userId, bool isNotifiableByEmail)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(azureConnection.ToString());
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(user.ContainerId);
            await blobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(fileStream);

            if (isNotifiableByEmail)
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
           
            StorageCredentials storageCredentials = new StorageCredentials(accountName, accountKey);          
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);   
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(thumbnailContainer);

            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;

            do
            {
                resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);

                foreach (var blobItem in resultSegment.Results)
                {
                    CloudBlockBlob blob = blobItem as CloudBlockBlob;
                    SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy
                    {
                        SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                        SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24),
                        Permissions = SharedAccessBlobPermissions.Read
                    };

                    string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
                    thumbnailUrls.Add(blob.Uri + sasBlobToken);

                }
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
