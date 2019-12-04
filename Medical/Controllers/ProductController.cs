using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Medical.Models;
using System.IO;
using Medical.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Medical.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace Medical.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(string key = "")
        {
            if (User.IsInRole(Global.ROLE_ADMIN) || User.IsInRole(Global.ROLE_CLERK))
                return RedirectToAction("Admin", new { key = key});
            IEnumerable<IGrouping<int, ProductModel>> arrProducts = _dbContext.Products.Where(product => product.product_name.Contains(key)).ToList().GroupBy(product => product.product_category);
            ViewData["arrCategories"] = _dbContext.Categories.ToList();
            return View(arrProducts);
        }
        
        [Authorize(Roles = Global.ROLE_ADMIN + "," + Global.ROLE_CLERK)]
        public IActionResult Admin(string key = "")
        {
            List<ProductModel> arrProducts = _dbContext.Products.Where(product => product.product_name.Contains(key)).ToList();
            List<SelectListItem> categoryLists = new List<SelectListItem>();
            List<CategoryModel> arrCategories = _dbContext.Categories.ToList();
            foreach (CategoryModel category in arrCategories)
            {
                categoryLists.Add(new SelectListItem(category.category_name, "" + category.category_id));
            }
            ViewData["arrCategories"] = categoryLists;
            return View(arrProducts);
        }

        [Authorize(Roles = Global.ROLE_ADMIN + "," + Global.ROLE_CLERK)]
        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel product)
        {
            string filePath = "";

            if (product.image != null && product.image.Length > 0)
            {
                string fileName = product.image.FileName;
                int nIdx = fileName.LastIndexOf('\\');
                nIdx = nIdx > 0 ? nIdx + 1 : 0;
                fileName = fileName.Substring(nIdx);

                filePath = "/uploads/products/" + Path.GetRandomFileName() + fileName;
                string fullPath = Path.GetFullPath("./wwwroot") + filePath;

                using (var stream = System.IO.File.Create(fullPath))
                {
                    await product.image.CopyToAsync(stream);
                }
            }

            ProductModel newProduct = new ProductModel()
            {
                product_id =  product.product_id,
                product_image = String.IsNullOrEmpty(filePath) ? product.product_image : filePath,
                product_name = product.product_name,
                product_category = product.product_category,
                product_description = Regex.Replace(product.product_description, @"\t|\n|\r", "    "),
                product_price = product.product_price
            };

            _dbContext.Products.Update(newProduct);
            _dbContext.SaveChanges();

            return Redirect("/Product");
        }

        [Authorize(Roles = Global.ROLE_ADMIN + "," + Global.ROLE_CLERK)]
        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                ProductModel product = _dbContext.Products.Find(id);
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        //Product Detail
        public IActionResult Detail(int id)
        {
            ProductModel product = _dbContext.Products.Find(id);
            CategoryModel category = _dbContext.Categories.Find(product.product_category);
            ViewData["Category"] = category == null ? "" : category.category_name;
            return View(product);
        }
    }
}
