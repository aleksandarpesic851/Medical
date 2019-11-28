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

namespace Medical.Controllers
{
    [Authorize(Roles = Global.ROLE_ADMIN + "," + Global.ROLE_CLERK)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<CategoryModel> arrCategories = _dbContext.Categories.ToList();
            return View(arrCategories);
        }



        [HttpPost]
        public async Task<IActionResult> Update(CategoryViewModel category)
        {
            string filePath = "";

            if (category.image != null && category.image.Length > 0)
            {
                filePath = "/uploads/categories/" + Path.GetRandomFileName() + category.image.FileName;
                string fullPath = Path.GetFullPath("./wwwroot") + filePath;

                using (var stream = System.IO.File.Create(fullPath))
                {
                    await category.image.CopyToAsync(stream);
                }
            }

            CategoryModel newCategory = new CategoryModel()
            {
                category_id = category.category_id,
                category_image = String.IsNullOrEmpty(filePath) ? category.category_image : filePath,
                category_name = category.category_name
            };

            _dbContext.Categories.Update(newCategory);
            _dbContext.SaveChanges();

            return Redirect("/Category");
        }

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                CategoryModel party = _dbContext.Categories.Find(id);
                _dbContext.Categories.Remove(party);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

    }
}
