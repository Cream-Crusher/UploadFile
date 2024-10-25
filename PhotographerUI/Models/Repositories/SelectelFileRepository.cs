using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace LimetimePhotoUploadUI.Models.Repositories
{
    public class SelectelFileRepository: ISelectelFileRepository
    {
        private string apiEndpoint;
        private string accessKey;
        private string secretKey;
        private string region;

        BasicAWSCredentials credentials;
        AmazonS3Config config;

        public SelectelFileRepository(string apiEndpoint, string accessKey, string secretKey)
        {
            this.apiEndpoint = apiEndpoint;
            this.accessKey = accessKey;
            this.secretKey = secretKey;

            this.credentials = new BasicAWSCredentials(this.accessKey, this.secretKey);
            this.config = new AmazonS3Config
            {
                ServiceURL = this.apiEndpoint,
                ForcePathStyle = true,
            };
        }

        
        public async Task Upload(string bucketName, string key, Stream content)
        {
            using (AmazonS3Client s3Client = new AmazonS3Client(this.credentials, this.config))
            {
                using (TransferUtility transferUtility = new TransferUtility(s3Client))
                {
                    try
                    {
                        await transferUtility.UploadAsync(content, bucketName, key);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        public async Task DeleteAsync(string bucketName, string key)
        {
            using (AmazonS3Client s3Client = new AmazonS3Client(this.credentials, this.config))
            {
                 DeleteObjectResponse response = await s3Client.DeleteObjectAsync(
                    new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = key,
                    });
            }
        }

        public async Task<Stream> GetAsync(string bucketName, string key)
        {
            using (AmazonS3Client s3Client = new AmazonS3Client(this.credentials, this.config))
            {
                GetObjectResponse response = await s3Client.GetObjectAsync(
                    new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = key,
                    });

                return response.ResponseStream;
            }
        }
    }
}
