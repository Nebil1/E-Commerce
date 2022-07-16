using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.Utility;

namespace E_Commerce.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticFiles.Role_Admin)]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitofwork;


    public CompanyController(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public IActionResult Index()
    {

        return View();
    }


    //GET
    public IActionResult UpdateCompany(int? id)
    {
        Company company = new();

        if (id == null || id == 0)
        {
            return View(company);
        }
        else
        {
            company = _unitofwork.Company.GetFirstorDefault(u => u.Id == id);
            return View(company);
        }
    }

    //POST
    [HttpPost]
    public IActionResult UpdateCompany(Company obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            if(obj.Id == 0)
            {
                _unitofwork.Company.Add(obj);
                TempData["success"] = "Company Profile created successfully";
            }
            else
            {
                _unitofwork.Company.Update(obj);
                TempData["success"] = "Company Profile updated successfully";
            }
            
            _unitofwork.Save();

            return RedirectToAction("Index");
        }

        return View(obj);
    }

    
    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList = _unitofwork.Company.GetAll();
        return Json(new { data = companyList });
    }

   
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitofwork.Company.GetFirstorDefault(u=>u.Id == id);

        if (obj == null)
        {
            return Json (new { success = false, message = "Error occured while deleting" });  
        }

        _unitofwork.Company.Remove(obj);
        _unitofwork.Save();

        return Json(new { success = true, message = "Deleted succesfully" });
       
    }
    #endregion

}





