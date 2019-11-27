using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class OrderModel
    {
        [Key]
        public int order_id { get; set; }
        public DateTime order_date { get; set; }
        public int order_customer { get; set; }
        public string order_address { get; set; }
        public string order_status { get; set; }
    }
}
