using Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Editorial.MVC.Models.Content
{
    public class ListViewModel
    {
        public IEnumerable<ContentModel> Contents { get; private set; }
           = new List<ContentModel>();

        public ListViewModel(List<ContentModel> contents)
        {
            Contents = contents;
        }
    }
}
