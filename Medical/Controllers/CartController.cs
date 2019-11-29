using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Medical.Models;
using Medical.Utility;
using Microsoft.AspNetCore.Authorization;

namespace Medical.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CartController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Global.ROLE_CUSTOMER)]
        [HttpPost]
        public bool AddToCart(CartModel cart)
        {
            if (cart.cart_product_count < 1)
                return false;
            cart.cart_customer = Convert.ToInt32(User.FindFirst("user_id").Value);
            CartModel prevCart = _dbContext.Carts.Where(e => e.cart_order == 0 && e.cart_product == cart.cart_product && e.cart_customer == cart.cart_customer).FirstOrDefault();
            if (prevCart == null)
            {
                _dbContext.Carts.Add(cart);
            }
            else
            {
                prevCart.cart_product_count += cart.cart_product_count;
                _dbContext.SaveChanges();
            }
            _dbContext.SaveChanges();
            return true;
        }

        public IActionResult GetInformation()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Ok(new
                {
                    item = 0,
                    amount = 0
                });
            }
            int customer = Convert.ToInt32(User.FindFirst("user_id").Value);

            List<CartModel> arrCarts = _dbContext.Carts.Where(e => e.cart_order == 0 && e.cart_customer == customer).ToList(); ;
            int nCnt = arrCarts.Count();
            int nAmount = 0;

            foreach(CartModel cart in arrCarts)
            {
                nAmount += (int)(cart.cart_product_count * _dbContext.Products.Find(cart.cart_product).product_price);
            }
            return Ok(new
            {
                item = nCnt,
                amount = nAmount
            });
        }
    }
}
