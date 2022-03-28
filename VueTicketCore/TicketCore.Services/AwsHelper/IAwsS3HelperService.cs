using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TicketCore.Services.AwsHelper
{
    public interface IAwsS3HelperService
    {
        Task<bool> UploadFileAsync(Stream fileStream, string fileName, string directory = null);
        Task<bool> UploadFileAsync(string filePath, string directory);
        Task<bool> UploadFileAsync(string contents, string contentType, string fileName, string directory);

        Task<(Stream FileStream, string ContentType)> ReadFileAsync(string fileName,
            string directory = null);
        Task<List<(Stream FileStream, string fileName, string ContentType)>> ReadDirectoryAsync(string directory);
        Task<bool> MoveFileAsync(string fileName, string sourceDirectory, string destDirectory);
        Task<bool> RemoveFileAsync(string fileName, string directory = null);

        string GeneratePreSignedUrlforPhotoAsync(string fileName, string bucketname, string directory = null);
        string GeneratePreSignedUrlAsync(string fileName, string serviceid, string directory = null);
        string GeneratePreSignedUrlAsync(string bucketname, string fileName, string serviceid, string directory = null);
    }
}