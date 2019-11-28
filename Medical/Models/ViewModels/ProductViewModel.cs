using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models.ViewModels
{
    public class ProductViewModel
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_image { get; set; }
        public float product_price { get; set; }
        public string product_description { get; set; }
        public int product_category { get; set; }
        public IFormFile image { get; set; }
    }
}
