using Microsoft.AspNetCore.Mvc;
using E_Commerce.DataAccess;
using E_Commerce.Models;
using System.Linq;
using E_Commerce.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.Utility;

namespace E_Commerce.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticFiles.Role_Admin)]
public class ShippingTypeController : Controller
{
    private readonly IUnitOfWork _unitofwork;

    public ShippingTypeController(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public IActionResult Index()
    {
        IEnumerable<ShippingType> objShippingTypeList = _unitofwork.ShippingType.GetAll();
        return View(objShippingTypeList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    public IActionResult Create(ShippingType obj)
    {

        if (ModelState.IsValid)
        {
            _unitofwork.ShippingType.Add(obj);
            //Save chages pushes the input data to the database
            _unitofwork.Save();
            TempData["success"] = "Created succesfully";
            return RedirectToAction("Index");
        }

        return View(obj);
    } 




    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var shippingtypeFromDb = _unitofwork.ShippingType.GetFirstorDefault(u => u.Id == id);

        //retrieve data from the database using entityframeworkcore
        if (shippingtypeFromDb == null)
        {
            return NotFound();
        }
        return View(shippingtypeFromDb);
    }

    //POST
    [HttpPost]
    public IActionResult Edit(ShippingType obj)
    {
        if (ModelState.IsValid)
        {
            _unitofwork.ShippingType.Update(obj);
            _unitofwork.Save();
            TempData["success"] = "Updated succesfully";

            return RedirectToAction("Index");
        }

        return View(obj);
    }


    //GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var shippingtypeFromDb = _unitofwork.ShippingType.GetFirstorDefault(u => u.Id == id);

        if (shippingtypeFromDb == null)
        {
            return NotFound();
        }
        return View(shippingtypeFromDb);
    }

    //POST
    [HttpPost]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitofwork.ShippingType.GetFirstorDefault(u => u.Id == id);

        if (obj == null)
        {
            return NotFound();
        }

        _unitofwork.ShippingType.Remove(obj);
        _unitofwork.Save();
        TempData["success"] = "Deleted succesfully";
        return RedirectToAction("Index");
    }

}





