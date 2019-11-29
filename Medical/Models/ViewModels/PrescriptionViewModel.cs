using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models.ViewModels
{
    public class PrescriptionViewModel
    {
        public string prescription_deliver_address { get; set; }
        public IFormFile prescription_image { get; set; }
    }
}
