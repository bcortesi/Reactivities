using Content.Models;
using System;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Content.Store.API.Models
{
    [DynamoDBTable("Contents")]
    public class ContentDBModel
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty]
        public string InBrief { get; set; }

        [DynamoDBProperty]
        public string FullText { get; set; }

        [DynamoDBProperty]
        public ContentStatus Status { get; set; }

        [DynamoDBProperty]
        public DateTime CreationDateTime { get; set; }

        [DynamoDBProperty]
        public DateTime PublicationDateTime { get; set; }

        [DynamoDBProperty]
        public ConfirmContentStatus ConfirmStatus { get; set; }
    }
}
