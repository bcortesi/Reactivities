using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Content.Store.API.Models
{
    public class AWSConfigSettings
    {
        public string AWSProfileName { get; set; }
        public string DynamoDBTableName { get; set; }
    }
}
