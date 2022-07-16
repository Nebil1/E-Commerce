using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Models;
using E_Commerce.Models.ViewModels;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace E_Commerce.Controllers;
[Area("User")]

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitofwork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitofwork)
        {
            _logger = logger;
            _unitofwork = unitofwork;   
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitofwork.Product.GetAll(includeProperties: "Category,ShippingType");
            return View(productList);
        }

        public IActionResult Electronics()
        {
            IEnumerable<Product> productList = _unitofwork.Product.GetAll(includeProperties: "Category,ShippingType");
            return View(productList);
        }

        public IActionResult Fashion()
        {
            IEnumerable<Product> productList = _unitofwork.Product.GetAll(includeProperties: "Category,ShippingType");
            return View(productList);
        }

        public IActionResult HealthandBeauty()
        {
            IEnumerable<Product> productList = _unitofwork.Product.GetAll(includeProperties: "Category,ShippingType");
            return View(productList);
        }

        public IActionResult Sports()
        {
            IEnumerable<Product> productList = _unitofwork.Product.GetAll(includeProperties: "Category,ShippingType");
            return View(productList);
        }

        public IActionResult Books()
        {
            IEnumerable<Product> productList = _unitofwork.Product.GetAll(includeProperties: "Category,ShippingType");
            return View(productList);
        }




    public IActionResult Details(int productId)
        {
        ShoppingCart cartObj = new()
        {
            Count = 1,
            ProductId = productId,
            Product = _unitofwork.Product.GetFirstorDefault(u => u.Id == productId, includeProperties: "Category,ShippingType"),
        };

        return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _unitofwork.ShoppingCart.GetFirstorDefault(
            u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);


        if (cartFromDb == null)
        {
            _unitofwork.ShoppingCart.Add(shoppingCart);
            _unitofwork.Save();
            HttpContext.Session.SetInt32(StaticFiles.SessionCart,
                _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
        }
        else
        {
            _unitofwork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
            _unitofwork.Save();
        }

         
        return RedirectToAction(nameof(Index));
    }



    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
