﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class CategoryModel
    {
        [Key]
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string category_image { get; set; }
    }
}
