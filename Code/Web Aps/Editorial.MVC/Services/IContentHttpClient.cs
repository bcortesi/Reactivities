using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace Editorial.MVC.Services
{
    public interface IContentHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
