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
using Medical.Models.ViewModels;

namespace Medical.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole(Global.ROLE_CUSTOMER))
                return RedirectToAction("Customer");
            return RedirectToAction("Admin");
        }

        //Create Order page
        [Authorize]
        public IActionResult Checkout(int nId = 0)
        {
            bool canMake = false;
            if (User.IsInRole(Global.ROLE_CUSTOMER))
            {
                nId = Convert.ToInt32(User.FindFirst("user_id").Value);
                canMake = true;
            }
            List<CartModel> arrCarts = _dbContext.Carts
                .Where(cart => cart.cart_customer == nId && cart.cart_order == 0)
                .ToList();
            List<ProductModel> arrProduct = _dbContext.Products.ToList();
            float totalPrice = 0;
            foreach (CartModel cart in arrCarts)
            {
                totalPrice += cart.cart_product_count * arrProduct.First(product => product.product_id == cart.cart_product).product_price;
            }
            ViewData["products"] = arrProduct;
            ViewData["canMake"] = canMake;
            ViewData["totalPrice"] = totalPrice;

            return View(arrCarts);
        }

        [Authorize(Roles = Global.ROLE_ADMIN + "," + Global.ROLE_CLERK)]
        public IActionResult Admin()
        {
            return View();
        }

        //Make Order
        [Authorize(Roles = Global.ROLE_CUSTOMER)]
        public IActionResult Make(OrderModel order)
        {
            int nId = Convert.ToInt32(User.FindFirst("user_id").Value);
            order.order_customer = nId;
            order.order_date = DateTime.Now;
            order.order_status = Global.ORDER_READY;
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            List<CartModel> arrCarts = _dbContext.Carts
                .Where(cart => cart.cart_customer == nId && cart.cart_order == 0)
                .ToList();
            foreach (CartModel cart in arrCarts)
            {
                cart.cart_order = order.order_id;
                _dbContext.Carts.Update(cart);
            }
            _dbContext.SaveChanges();
            return RedirectToAction("OrderDetail", new { id=order.order_id});
        }

        [Authorize]
        public IActionResult OrderList()
        {
            List<OrderModel> arrOrders = new List<OrderModel>();

            //Cutomer can see his orders
            if (User.IsInRole(Global.ROLE_CUSTOMER))
                arrOrders = _dbContext.Orders.Where(order =>
                                order.order_status != Global.ORDER_DELIVERED &&
                                order.order_status != Global.ORDER_RETURNED &&
                                order.order_customer == Convert.ToInt32(User.FindFirst("user_id").Value)
                            ).ToList();
                
            //Delivery body can see orders, which is assigned to him
            else if (User.IsInRole(Global.ROLE_DELIVERY))
                arrOrders = _dbContext.Orders.Where(order =>
                                order.order_status != Global.ORDER_DELIVERED &&
                                order.order_status != Global.ORDER_RETURNED &&
                                order.order_delivery == Convert.ToInt32(User.FindFirst("user_id").Value)
                            ).ToList();
            else
                arrOrders = _dbContext.Orders.Where(order =>
                                order.order_status != Global.ORDER_DELIVERED &&
                                order.order_status != Global.ORDER_RETURNED
                            ).ToList();


            List<ProductModel> arrProduct = _dbContext.Products.ToList();
            List<OrderPriceModel> arrPrice = new List<OrderPriceModel>();

            foreach(OrderModel order in arrOrders)
            {
                List<CartModel> arrCart = _dbContext.Carts.Where(cart => cart.cart_order == order.order_id).ToList();
                float totalPrice = 0;
                foreach (CartModel cart in arrCart)
                {
                    totalPrice += cart.cart_product_count * arrProduct.First(product => product.product_id == cart.cart_product).product_price;
                }
                arrPrice.Add(new OrderPriceModel
                {
                    orderId = order.order_id,
                    totalAmount = totalPrice
                });
            }

            ViewData["arrUsers"] = _dbContext.Users.ToList();
            ViewData["arrPrice"] = arrPrice;

            return View(arrOrders);
        }


        [Authorize(Roles = Global.ROLE_MANAGER + "," + Global.ROLE_ADMIN)]
        public IActionResult Transaction()
        {
            List<OrderModel> arrOrders = _dbContext.Orders.Where(order =>
                                order.order_status == Global.ORDER_DELIVERED ||
                                order.order_status == Global.ORDER_RETURNED
                            ).ToList();

            ViewData["arrUsers"] = _dbContext.Users.ToList();
            return View(arrOrders);
        }

        //Current Order
        [Authorize]
        public IActionResult OrderDetail(int id)
        {
            // it means that current user is customer, who wants to see his order details
            if (id == 0)
            {
                return View("Empty");
            }

            OrderModel order = _dbContext.Orders.Find(id);
            List<CartModel> arrCart = _dbContext.Carts.Where(cart => cart.cart_order == id).ToList();
            List<ProductModel> arrProduct = _dbContext.Products.ToList();
            float totalPrice = 0;
            foreach(CartModel cart in arrCart)
            {
                totalPrice += cart.cart_product_count * arrProduct.First(product => product.product_id == cart.cart_product).product_price;
            }
            ViewData["arrUsers"] = _dbContext.Users.ToList();
            ViewData["totalPrice"] = totalPrice;
            ViewData["arrCart"] = arrCart;
            ViewData["arrProduct"] = arrProduct;
            ViewData["customer"] = _dbContext.Users.Find(order.order_customer).user_name;
            if(order.order_prescription > 0)
            {
                ViewData["prescription"] = _dbContext.Prescriptions.Find(order.order_prescription).prescription_image;
            }
            return View(order);
        }

        //Assign order to delivery body
        [Authorize(Roles = Global.ROLE_CLERK + "," + Global.ROLE_ADMIN)]
        [HttpPost]
        public bool AssignOrder(OrderModel order)
        {
            OrderModel currentOrder = _dbContext.Orders.Find(order.order_id);
            currentOrder.order_status = Global.ORDER_PICKED_UP;
            currentOrder.order_delivery = order.order_delivery;
            _dbContext.Orders.Update(currentOrder);
            _dbContext.SaveChanges();
            return true;
        }

        //Assign order to delivery body
        [Authorize(Roles = Global.ROLE_CLERK + "," + Global.ROLE_DELIVERY + "," + Global.ROLE_ADMIN)]
        [HttpPost]
        public bool ChangeStatus(OrderModel order)
        {
            OrderModel currentOrder = _dbContext.Orders.Find(order.order_id);
            currentOrder.order_status = order.order_status;
            _dbContext.Orders.Update(currentOrder);
            _dbContext.SaveChanges();
            return true;
        }

        [Authorize(Roles = Global.ROLE_ADMIN + "," + Global.ROLE_MANAGER + "," + Global.ROLE_CLERK)]
        public bool CreateOrder(PrescriptionOrderViewModel model)
        {
            if (model.ids.Count() == 0)
                return false;

            //Create order
            PrescriptionModel prescription = _dbContext.Prescriptions.Find(model.prescription);

            OrderModel order = new OrderModel
            {
                order_date = DateTime.Now,
                order_customer = model.customer,
                order_address = prescription.prescription_address,
                order_status = Global.ORDER_READY,
                order_prescription = model.prescription
            };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            //Create Cart from products
            int i = 0;
            foreach(int id in model.ids)
            {
                CartModel cart = new CartModel
                {
                    cart_customer = model.customer,
                    cart_product = id,
                    cart_product_count = model.vals.ElementAt(i),
                    cart_order = order.order_id
                };
                _dbContext.Carts.Add(cart);
                i++;
            }
            _dbContext.SaveChanges();

            //Update prescription
            prescription.prescription_order = order.order_id;
            _dbContext.Prescriptions.Update(prescription);
            _dbContext.SaveChanges();
            return true;
        }

        [Authorize(Roles = Global.ROLE_CUSTOMER)]
        public bool CashPay(int orderId)
        {
            OrderModel order = _dbContext.Orders.Find(orderId);
            if (order == null)
                return false;
            order.order_status = Global.ORDER_DELIVERED;
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
            return true;
        }

        //Online Payment
        public bool OnlinePay()
        {
            return true;
        }
    }
}
