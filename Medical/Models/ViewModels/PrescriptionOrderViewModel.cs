using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models.ViewModels
{
    public class PrescriptionOrderViewModel
    {
        public int prescription { get; set; }
        public int customer { get; set; }
        public IEnumerable<int> ids { get; set; }
        public IEnumerable<int> vals { get; set; }
    }
}
