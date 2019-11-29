using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Medical.Models;

namespace Medical.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<IGrouping<int, ProductModel>> arrProducts = _dbContext.Products.ToList().GroupBy(product => product.product_category);
            ViewData["arrCategories"] = _dbContext.Categories.OrderByDescending(e => e.category_id).ToList();

            return View(arrProducts);
        }
    }
}
