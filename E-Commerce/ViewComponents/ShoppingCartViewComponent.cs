using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitofwork;
        public ShoppingCartViewComponent(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(StaticFiles.SessionCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(StaticFiles.SessionCart));
                }
                else
                {
                    HttpContext.Session.SetInt32(StaticFiles.SessionCart,
                        _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(StaticFiles.SessionCart));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
