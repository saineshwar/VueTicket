using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TicketCore.Common;

namespace TicketCore.Services.AwsHelper
{
    public class AwsS3HelperService : IAwsS3HelperService
    {
        private readonly ILogger<AwsS3HelperService> _logger;
        private readonly AwsSettings _awsSettings;
        private readonly IAmazonS3 _s3Client;

        public AwsS3HelperService(IOptions<AwsSettings> awsSettings, AwsS3BucketOptions s3BucketOptions,
            ILogger<AwsS3HelperService> logger, IAmazonS3 s3Client)
        {

            _logger = logger;
            _s3Client = s3Client;
            _awsSettings = awsSettings.Value;

        }

        /// <summary>
        /// uploads file to s3 bucket using file stream
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        /// <param name="serviceid"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(Stream fileStream, string fileName,
            string directory = null)
        {
            try
            {
                string bucketPath;
                using (var fileTransferUtility = new TransferUtility(_s3Client))
                {

                    bucketPath = $@"{_awsSettings.BucketName}/{directory}";

                    var fileUploadRequest = new TransferUtilityUploadRequest()
                    {
                        CannedACL = S3CannedACL.Private,
                        BucketName = bucketPath,
                        Key = fileName,
                        InputStream = fileStream
                    };

                    await fileTransferUtility.UploadAsync(fileUploadRequest);
                }


                return true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.LogError("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError(
                        $"An error occurred with the message '{amazonS3Exception.Message}' when uploading {fileName}");
                }

                return false;
            }
        }

        /// <summary>
        /// uploads file to s3 bucket from specified file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(string filePath, string directory = null)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);
                var bucketPath = !string.IsNullOrWhiteSpace(directory)
                    ? _awsSettings.BucketName + @"/" + directory
                    : _awsSettings.BucketName;
                // 1. Upload a file, file name is used as the object key name.
                var fileUploadRequest = new TransferUtilityUploadRequest()
                {
                    CannedACL = S3CannedACL.Private,
                    BucketName = bucketPath,
                    FilePath = filePath,
                };

                await fileTransferUtility.UploadAsync(fileUploadRequest);

                return true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.LogError("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError(
                        $"An error occurred with the message '{amazonS3Exception.Message}' when uploading {filePath}");
                }

                return false;
            }
        }

        /// <summary>
        /// writes file to s3 bucket using specified contents, content type
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(string contents, string contentType, string fileName,
            string directory = null)
        {
            try
            {



                var bucketPath = !string.IsNullOrWhiteSpace(directory)
                    ? _awsSettings.BucketName + @"/" + directory
                    : _awsSettings.BucketName;
                //1. put object 
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketPath,
                    Key = fileName,
                    ContentBody = contents,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.Private
                };
                var response = await _s3Client.PutObjectAsync(putRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                return false;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError(
                        $"An error occurred with the message '{amazonS3Exception.Message}' when writing {fileName}");
                }

                return false;
            }
        }

        /// <summary>
        /// returns file stream from s3 bucket
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="serviceId"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public async Task<(Stream FileStream, string ContentType)> ReadFileAsync(string fileName,
            string directory = null)
        {
            try
            {
                GetObjectResponse objectResponse;
                using (var fileTransferUtility = new TransferUtility(_s3Client))
                {
                    var bucketPath = $@"{_awsSettings.BucketName}/{directory}";
                    var request = new GetObjectRequest()
                    {
                        BucketName = bucketPath,
                        Key = fileName
                    };
                    // 1. read files
                    objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(request);
                }

                return (objectResponse.ResponseStream, objectResponse.Headers.ContentType);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.LogError("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError("An error occurred with the message '{0}' when reading an object",
                        amazonS3Exception.Message);
                }

                return (null, null);
            }
        }

        /// <summary>
        /// returns files from s3 folder
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public async Task<List<(Stream FileStream, string fileName, string ContentType)>> ReadDirectoryAsync(
            string directory)
        {
            var objectCollection = new List<(Stream, string, string)>();
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);
                var request = new ListObjectsRequest()
                {
                    BucketName = _awsSettings.BucketName,
                    Prefix = directory
                };
                // 1. read files
                var objectResponse = await fileTransferUtility.S3Client.ListObjectsAsync(request);
                foreach (var entry in objectResponse.S3Objects)
                {
                    var fileName = entry.Key.Split('/').Last();
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var response = await ReadFileAsync(fileName, directory);
                        objectCollection.Add((response.FileStream, fileName, response.ContentType));
                    }
                }

                return objectCollection;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.LogError("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError("An error occurred with the message '{0}' when reading an object",
                        amazonS3Exception.Message);
                }

                return objectCollection;
            }
        }

        /// <summary>
        /// moves file (object) between bucket folders
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        /// <returns></returns>
        public async Task<bool> MoveFileAsync(string fileName, string sourceDirectory, string destDirectory)
        {
            try
            {
                var copyRequest = new CopyObjectRequest
                {
                    SourceBucket = _awsSettings.BucketName + @"/" + sourceDirectory,
                    SourceKey = fileName,
                    DestinationBucket = _awsSettings.BucketName + @"/" + destDirectory,
                    DestinationKey = fileName
                };



                var response = await _s3Client.CopyObjectAsync(copyRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    var deleteRequest = new DeleteObjectRequest
                    {
                        BucketName = _awsSettings.BucketName + @"/" + sourceDirectory,
                        Key = fileName
                    };
                    await _s3Client.DeleteObjectAsync(deleteRequest);
                    return true;
                }

                return false;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError("An error occurred with the message '{0}' when moving object",
                        amazonS3Exception.Message);
                }

                return false;
            }
        }

        /// <summary>
        /// removes file from s3 bucket
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="serviceId"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public async Task<bool> RemoveFileAsync(string fileName, string directory = null)
        {
            try
            {
                string bucketPath;
                using (var fileTransferUtility = new TransferUtility(_s3Client))
                {
                    bucketPath = $@"{_awsSettings.BucketName}/{directory}";
                    // 1. deletes files
                    await fileTransferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
                    {
                        BucketName = bucketPath,
                        Key = fileName
                    });
                }


                return true;
            }
            catch (AmazonS3Exception s3Exception)
            {
                if (s3Exception.ErrorCode != null &&
                    (s3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     s3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.LogError("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError(s3Exception.Message,
                        s3Exception.InnerException);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);

                throw;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="serviceid"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public string GeneratePreSignedUrlAsync(string fileName, string serviceid, string directory = null)
        {
            try
            {
                var bucketPath = $@"{_awsSettings.BucketName}/{directory}/{serviceid}";
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketPath,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.Now.AddMinutes(30),
                    Key = fileName
                };

                var url = _s3Client.GetPreSignedURL(request);
                return url;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.LogError("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError(
                        $"An error occurred with the message '{amazonS3Exception.Message}' when uploading {fileName}");
                }

                return string.Empty;
            }
        }

        public string GeneratePreSignedUrlAsync(string bucketname, string fileName, string serviceid, string directory = null)
        {
            try
            {
                var bucketPath = $@"{bucketname}/{directory}/{serviceid}";
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketPath,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.Now.AddMinutes(30),
                    Key = fileName
                };

                var url = _s3Client.GetPreSignedURL(request);
                return url;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.LogError("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError(
                        $"An error occurred with the message '{amazonS3Exception.Message}' when uploading {fileName}");
                }

                return string.Empty;
            }
        }

        public string GeneratePreSignedUrlforPhotoAsync(string fileName, string bucketname, string directory = null)
        {
            try
            {
                var bucketPath = $@"{bucketname}/{directory}";
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketPath,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.Now.AddHours(24),
                    Key = fileName
                };

                var url = _s3Client.GetPreSignedURL(request);
                return url;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.LogError("Please check the provided AWS Credentials.");
                }
                else
                {
                    _logger.LogError(
                        $"An error occurred with the message '{amazonS3Exception.Message}' when uploading {fileName}");
                }

                return string.Empty;
            }
        }

        //public string GetFileUrlAsync(string fileName, string directory = null)
        //{
        //    var bucketPath = !string.IsNullOrWhiteSpace(directory)
        //        ? _awsSettings.BucketName + @"/" + directory
        //        : _awsSettings.BucketName;


        //}
    }
}