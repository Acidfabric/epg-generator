﻿using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using awscsharp.Models;
using Microsoft.Extensions.Options;

namespace awscsharp.S3Uploader
{
    public class S3Uploader : IS3Uploader
    {
        private IAmazonS3 _client { get; set; }
        private IOptions<AwsConfig> _config { get; set; }

        public S3Uploader(IAmazonS3 client, IOptions<AwsConfig> config)
        {
            _config = config;
            _client = client;
        }

        public async Task WritingAnObjectAsync(string generatedEpg)
        {
            try
            {
                var uploadRequest = new PutObjectRequest
                {
                    BucketName = _config.Value.BucketName,
                    Key = _config.Value.FileName,
                    ContentBody = generatedEpg,
                    ContentType = "text/xml"
                };

                uploadRequest.Metadata.Add("x-amz-meta-generatedAt", DateTime.Now.ToShortDateString());

                await _client.PutObjectAsync(uploadRequest);

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}
