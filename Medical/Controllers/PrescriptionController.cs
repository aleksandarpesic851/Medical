using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Medical.Models;
using Microsoft.AspNetCore.Authorization;
using Medical.Utility;
using Medical.Models.ViewModels;
using System.IO;

namespace Medical.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PrescriptionController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = Global.ROLE_ADMIN + "," + Global.ROLE_CLERK)]
        public IActionResult Index()
        {
            List<PrescriptionModel> arrPrescription = _dbContext.Prescriptions.Where(e => e.prescription_order == 0).ToList();
            ViewData["arrUsers"] = _dbContext.Users.ToList();
            return View(arrPrescription);
        }
        
        //Create Prescription by Customer
        [Authorize(Roles = Global.ROLE_CUSTOMER)]
        public async Task<bool> Create(PrescriptionViewModel model)
        {
            if (model.prescription_image.Length < 1)
                return false;
            string fileName = model.prescription_image.FileName;
            int nIdx = fileName.LastIndexOf('\\');
            nIdx = nIdx > 0 ? nIdx + 1 : 0;
            fileName = fileName.Substring(nIdx);
            string filePath = "/uploads/prescription/" + Path.GetRandomFileName() + fileName;
            string fullPath = Path.GetFullPath("./wwwroot") + filePath;

            using (var stream = System.IO.File.Create(fullPath))
            {
                await model.prescription_image.CopyToAsync(stream);
            }

            PrescriptionModel prescription = new PrescriptionModel
            {
                prescription_customer = Convert.ToInt32(User.FindFirst("user_id").Value),
                prescription_image = filePath,
                prescription_date = DateTime.Now,
                prescription_address = model.prescription_deliver_address
            };
            _dbContext.Prescriptions.Add(prescription);
            _dbContext.SaveChanges();
            return true;
        }

        public IActionResult Detail(int id)
        {
            PrescriptionModel prescription = _dbContext.Prescriptions.Find(id);
            UserModel user = _dbContext.Users.Find(prescription.prescription_customer);
            
            //if this prescription is not ordered, send all product lists, so that clerk can set medecine
            if (prescription.prescription_order == 0)
            {
                List<ProductModel> arrProducts = _dbContext.Products.ToList();
                ViewData["arrProducts"] = arrProducts;

                List<CategoryModel> arrCategory = _dbContext.Categories.ToList();
                ViewData["arrCategory"] = arrCategory;
            }
            ViewData["customer"] = user.user_name;
            return View(prescription);
        }
    }
}
