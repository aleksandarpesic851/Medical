using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class CartModel
    {
        [Key]
        public int cart_id { get; set; }
        public int cart_customer { get; set; }
        public int cart_order { get; set; }
        public int cart_product { get; set; }
        public int cart_product_count { get; set; }

    }
}
