using AutoMapper;
using Content.Models;
using Content.Store.API.Models;
using Content.Store.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Content.Store.API.Services.Implementations
{
    public class S3ContentStorage : IContentStorageService
    {
        private readonly IMapper _mapper;
        private readonly IOptions<AWSConfigSettings> appSettings;
        public S3ContentStorage(IMapper mapper, IOptions<AWSConfigSettings> app)
        {
            _mapper = mapper;
            appSettings = app;
        }

        public Task<AWSResponse> AddAsync(ContentModel model)
        {
            throw new NotImplementedException();
        }

        public Task ConfirmAsync(ConfirmContentModel model)
        {
            throw new NotImplementedException();
        }

        public Task<List<ContentModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ContentModel> GetAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<AWSResponse> UpdateAsync(ContentModel model)
        {
            throw new NotImplementedException();
        }
    }
}
