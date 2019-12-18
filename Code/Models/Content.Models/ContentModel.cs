using System;

namespace Content.Models
{
    public class ContentModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string InBrief { get; set; }
        public string FullText { get; set; }
        public ContentStatus Status { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime PublicationDateTime { get; set; }
    }
}
