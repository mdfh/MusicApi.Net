using Azure.Storage.Blobs;

namespace MusicApi.Helper
{
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IFormFile file)
            {
            /**** Azure File upload code... *****/
            //// Prepare connection strings
            // string connectionString = @"";
            // string containerName = "";

            //// Instantiate Azure Blob Client
            //var blobContainerClient = new BlobContainerClient(connectionString, containerName);
            //var blobClient = blobContainerClient.GetBlobClient(file.FileName);

            //var memoryStream = new MemoryStream();
            //await file.CopyToAsync(memoryStream);

            //// Set to initial position after copying
            //memoryStream.Position = 0;

            //// Upload image
            //await blobClient.UploadAsync(memoryStream);
            //var imageUrl = blobClient.Uri.AbsoluteUri;
            //return imageUrl;

            return "dummy_image_url";
            }
    }
}
