using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_image { get; set; }
        public IFormFile image { get; set; }
    }
}
