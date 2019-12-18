using Content.Models;
using Content.Store.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Content.Store.API.Services.Interfaces
{
    public interface IContentStorageService
    {
        Task<AWSResponse> AddAsync(ContentModel model);
        Task<AWSResponse> UpdateAsync(ContentModel model);

        Task<ContentModel> GetAsync(string Id);
        Task<List<ContentModel>> GetAllAsync();        
        Task ConfirmAsync(ConfirmContentModel model);
    }
}
