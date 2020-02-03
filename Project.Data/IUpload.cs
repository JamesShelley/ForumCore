using Microsoft.WindowsAzure.Storage.Blob;

namespace Project.Data
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString, string containerName);

    }
}