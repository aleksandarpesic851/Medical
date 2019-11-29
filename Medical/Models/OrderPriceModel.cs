using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class OrderPriceModel
    {
        public int orderId { get; set; }
        public float totalAmount { get; set; }
    }
}
