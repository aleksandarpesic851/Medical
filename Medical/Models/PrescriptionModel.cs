using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class PrescriptionModel
    {
        [Key]
        public int prescription_id { get; set; }
        public int prescription_customer { get; set; }
        public string prescription_image { get; set; }
        public DateTime prescription_date { get; set; }
        public int prescription_order { get; set; }
        public string prescription_address { get; set; }
    }
}
