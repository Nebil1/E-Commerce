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
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitofwork;

    public CategoryController(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitofwork.Category.GetAll();
       
        return View(objCategoryList);
    }


    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    public IActionResult Create(Category obj)
    {

        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The Name can't be the same as the DisplayOrder");
        }


        if (ModelState.IsValid)
        {
            _unitofwork.Category.Add(obj);
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

       var categoryFirstFromDb = _unitofwork.Category.GetFirstorDefault(u => u.Id == id);
      
        //var categorySingleFromDb = _db.Categories.SingleOrDefault(u => u.Id == id);
        //var categoryFromDb = _db.Categories.Find(id);


        //retrieve data from the database using entityframeworkcore
        if (categoryFirstFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFirstFromDb);
    }

    //POST
    [HttpPost]
    public IActionResult Edit(Category obj)
    {

        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The Name can't be the same as the DisplayOrder");
        }


        if (ModelState.IsValid)
        {
            _unitofwork.Category.Update(obj);
            //Save chages pushes the input data to the database
            _unitofwork.Save();
            TempData["success"] = "Updated succesfully";
            return RedirectToAction("Index");
        }

        if (obj.Name == null || obj.DisplayOrder.ToString() == null)
        {
            ModelState.AddModelError("Name", "The Name field can't be empty");
            ModelState.AddModelError("DisplayOrder", "The Display Order field can't be empty");
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

        var categoryFromDb = _unitofwork.Category.GetFirstorDefault(u => u.Id == id);
        //var categoryFromDb = _db.Categories.Find(id);

        //retrieve data from the database using entityframeworkcore
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    //POST
    [HttpPost]
    public IActionResult DeletePost(int? id)
    {
        var obj = _unitofwork.Category.GetFirstorDefault(u => u.Id == id);

        if (obj == null)
        {
            return NotFound();
        }

        _unitofwork.Category.Remove(obj);
        _unitofwork.Save();
        TempData["success"] = "Deleted succesfully";
        return RedirectToAction("Index");
    }

}





