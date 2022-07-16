using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.Utility;

namespace E_Commerce.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticFiles.Role_Admin)]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitofwork;

    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IUnitOfWork unitofwork, IWebHostEnvironment webHostEnvironment)
    {
        _unitofwork = unitofwork;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {

        return View();
    }

   

    //GET
    public IActionResult UpdateProduct(int? id)
    {
        ProductViewModel productView = new()
        {
            Product = new(),

            CategoryList = _unitofwork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),

            ShippingTypeList = _unitofwork.ShippingType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),

        };

        //create product 
        if (id == null || id == 0)
        {
            //ViewBag.CategoryList = CategoryList;
            //ViewData["ShippingTypeList"] = ShippingTypeList;
            return View(productView);
        }

        //update product 
        else
        {
            productView.Product = _unitofwork.Product.GetFirstorDefault(u => u.Id == id);
            return View(productView);
        }
    }

    //POST
    [HttpPost]
    public IActionResult UpdateProduct(ProductViewModel obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                if(obj.Product.Image != null)
                {
                    var oldImagepath = Path.Combine(wwwRootPath, obj.Product.Image.TrimStart('\\'));
                    if(System.IO.File.Exists(oldImagepath))
                    {
                        System.IO.File.Delete(oldImagepath);
                    }
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.Product.Image = @"\images\products\" + fileName + extension;

            }

            if(obj.Product.Id == 0)
            {
                _unitofwork.Product.Add(obj.Product);
                _unitofwork.Save();
                TempData["success"] = "Product added successfully";
            }
            else
            {
                _unitofwork.Product.Update(obj.Product);
                _unitofwork.Save();
                TempData["success"] = "Product updated successfully";
            }
            
            return RedirectToAction("Index");
        }

        return View(obj);
    }


    
    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitofwork.Product.GetAll(includeProperties: "Category,ShippingType");
        return Json(new { data = productList });
    }

   
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitofwork.Product.GetFirstorDefault(u=>u.Id == id);

        if (obj == null)
        {
            return Json (new { success = false, message = "Error occured while deleting" });  
        }

        var oldImagepath = Path.Combine(_webHostEnvironment.WebRootPath, obj.Image.TrimStart('\\'));

        if (System.IO.File.Exists(oldImagepath))
        {
            System.IO.File.Delete(oldImagepath);
        }

        _unitofwork.Product.Remove(obj);
        _unitofwork.Save();

        return Json(new { success = true, message = "Deleted succesfully" });
       
    }
    #endregion

}





