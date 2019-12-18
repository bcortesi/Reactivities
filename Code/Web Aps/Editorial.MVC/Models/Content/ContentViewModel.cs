using Content.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Editorial.MVC.Models.Content
{
    public class ContentViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string InBrief { get; set; }
        
        public string FullText { get; set; }
        
        public ContentStatus Status { get; set; }
        
        public DateTime PublicationDateTime { get; set; }
    }

    
}
