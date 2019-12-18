using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Content.Store.API.Models
{
    public class AWSResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }
}
