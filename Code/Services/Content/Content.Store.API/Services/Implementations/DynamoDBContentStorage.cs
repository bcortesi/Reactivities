using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using AutoMapper;
using Content.Models;
using Content.Store.API.Models;
using Content.Store.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Content.Store.API.Services.Implementations
{
    public class DynamoDBContentStorage : IContentStorageService
    {
        private readonly IMapper _mapper;
        private readonly IOptions<AWSConfigSettings> appSettings;
        public DynamoDBContentStorage(IMapper mapper, IOptions<AWSConfigSettings> app)
        {
            _mapper = mapper;
            appSettings = app;
        }

        private AmazonDynamoDBClient GetAWSClient()
        {
            var sharedFile = new SharedCredentialsFile();
            if (sharedFile.TryGetProfile(appSettings.Value.AWSProfileName, out CredentialProfile mlexProfile) &&
                AWSCredentialsFactory.TryGetAWSCredentials(mlexProfile, sharedFile, out AWSCredentials awsCredentials))
            {
                return new AmazonDynamoDBClient(awsCredentials, mlexProfile.Region);
            }
            else
            {
                throw new Exception("AWS credentials can not be fetched.");
            }            
        }

        public async Task<AWSResponse> AddAsync(ContentModel model)
        {
            try
            {
                var dbModel = _mapper.Map<ContentDBModel>(model);

                dbModel.Id = Guid.NewGuid().ToString();
                dbModel.CreationDateTime = DateTime.UtcNow;
                dbModel.ConfirmStatus = ConfirmContentStatus.Pending;
                                
                using (var client = GetAWSClient())
                {
                    var table = await client.DescribeTableAsync(appSettings.Value.DynamoDBTableName);

                    using (var context = new DynamoDBContext(client))
                    {
                        await context.SaveAsync(dbModel);
                    }
                }
                return new AWSResponse
                {
                    Message = dbModel.Id,
                    Status = HttpStatusCode.OK
                };
            }
            catch (AmazonDynamoDBException ex)
            {
                Console.WriteLine("Error '{0}' encountered on server.Message: '{1}' when inserting content to table '{2}'", ex.StatusCode, ex.Message, appSettings.Value.DynamoDBTableName);

                return new AWSResponse
                {
                    Message = ex.Message,
                    Status = ex.StatusCode
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered on server.Message: '{0}' when inserting content to table '{1}'", ex.Message, appSettings.Value.DynamoDBTableName);

                return new AWSResponse
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<AWSResponse> UpdateAsync(ContentModel model)
        {
            try
            {
                var dbModel = _mapper.Map<ContentDBModel>(model);

                
                using (var client = GetAWSClient())
                {
                    var table = await client.DescribeTableAsync(appSettings.Value.DynamoDBTableName);

                    using (var context = new DynamoDBContext(client))
                    {
                        await context.SaveAsync(dbModel);
                    }
                }
                return new AWSResponse
                {
                    Message = dbModel.Id,
                    Status = HttpStatusCode.OK
                };
            }
            catch (AmazonDynamoDBException ex)
            {
                Console.WriteLine("Error '{0}' encountered on server.Message: '{1}' when inserting content to table '{2}'", ex.StatusCode, ex.Message, appSettings.Value.DynamoDBTableName);

                return new AWSResponse
                {
                    Message = ex.Message,
                    Status = ex.StatusCode
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered on server.Message: '{0}' when inserting content to table '{1}'", ex.Message, appSettings.Value.DynamoDBTableName);

                return new AWSResponse
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ContentModel> GetAsync(string Id)
        {
            using (var client = GetAWSClient())
            {
                using var context = new DynamoDBContext(client);
                var dbModel = await context.LoadAsync<ContentDBModel>(Id);
                if (dbModel != null) return _mapper.Map<ContentModel>(dbModel);
            }

            throw new KeyNotFoundException();
        }

        public async Task<List<ContentModel>> GetAllAsync()
        {
            using (var client = GetAWSClient())
            {
                using var context = new DynamoDBContext(client);
                var scanResult = await context.ScanAsync<ContentDBModel>(new List<ScanCondition>()).GetNextSetAsync();
                return scanResult.Select(item => _mapper.Map<ContentModel>(item)).ToList();
            }
        }
        public async Task ConfirmAsync(ConfirmContentModel model)
        {
            throw new NotImplementedException();
        }
    }
}
