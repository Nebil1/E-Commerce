using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Models.ViewModels;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using E_Commerce.Models;
using Stripe.Checkout;

namespace E_Commerce.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int Total { get; set; }

        public CartController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
                OrderType = new()
            };

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.ListPrice100);
                ShoppingCartVM.OrderType.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
                OrderType = new()
            };
            ShoppingCartVM.OrderType.ApplicationUser = _unitofwork.ApplicationUser.GetFirstorDefault(
                u => u.Id == claim.Value);

            ShoppingCartVM.OrderType.Name = ShoppingCartVM.OrderType.ApplicationUser.Name;
            ShoppingCartVM.OrderType.PhoneNumber = ShoppingCartVM.OrderType.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderType.Address = ShoppingCartVM.OrderType.ApplicationUser.Address;
            ShoppingCartVM.OrderType.City = ShoppingCartVM.OrderType.ApplicationUser.City;
            ShoppingCartVM.OrderType.Region = ShoppingCartVM.OrderType.ApplicationUser.Region;
            ShoppingCartVM.OrderType.PostalCode = ShoppingCartVM.OrderType.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.ListPrice100);
                ShoppingCartVM.OrderType.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product");

            ShoppingCartVM.OrderType.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderType.ApplicationUserId = claim.Value;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.ListPrice100);
                ShoppingCartVM.OrderType.OrderTotal += (cart.Price * cart.Count);
            }

            ApplicationUser applicationUser = _unitofwork.ApplicationUser.GetFirstorDefault(u => u.Id == claim.Value);
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			{

                ShoppingCartVM.OrderType.PaymentStatus = StaticFiles.PaymentStatusPending;
                ShoppingCartVM.OrderType.OrderStatus = StaticFiles.StatusPending;
            }
			else
			{
                ShoppingCartVM.OrderType.PaymentStatus = StaticFiles.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderType.OrderStatus = StaticFiles.StatusApproved;
            }

            _unitofwork.OrderType.Add(ShoppingCartVM.OrderType);
            _unitofwork.Save();

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderType.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitofwork.OrderDetail.Add(orderDetail);
                _unitofwork.Save();
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {


                //stripe payment settings
                var domain = "https://localhost:7034/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"User/cart/OrderConfirmation?id={ShoppingCartVM.OrderType.Id}",
                    CancelUrl = domain + $"User/cart/index",
                };

                foreach (var item in ShoppingCartVM.ListCart)
                {

                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),//20.00 -> 2000
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.ProductName
                            },

                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sessionLineItem);

                }
                var service = new SessionService();
                Session session = service.Create(options);
                _unitofwork.OrderType.UpdateStripePaymentID(ShoppingCartVM.OrderType.Id, session.Id, session.PaymentIntentId);
                _unitofwork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
			else
			{
                return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoppingCartVM.OrderType.Id });
            }
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderType orderType = _unitofwork.OrderType.GetFirstorDefault(u => u.Id == id, includeProperties: "ApplicationUser");
            if (orderType.PaymentStatus != StaticFiles.PaymentStatusDelayedPayment)
			{
                var service = new SessionService();
                Session session = service.Get(orderType.SessionId);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitofwork.OrderType.UpdateStatus(id, StaticFiles.StatusApproved, StaticFiles.PaymentStatusApproved);
                    _unitofwork.Save();
                }
            }

                
            List<ShoppingCart> shoppingCarts = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId ==
                 orderType.ApplicationUserId).ToList();
            _unitofwork.ShoppingCart.RemoveRange(shoppingCarts);
			_unitofwork.Save();

            return View(id);
        }



        //cart Increment,Decrement and Remove
        public IActionResult Plus(int cartId)
        {
            var cart = _unitofwork.ShoppingCart.GetFirstorDefault(u => u.Id == cartId);
            _unitofwork.ShoppingCart.IncrementCount(cart, 1);
            _unitofwork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitofwork.ShoppingCart.GetFirstorDefault(u => u.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitofwork.ShoppingCart.Remove(cart);
                var count = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count - 1;
                HttpContext.Session.SetInt32(StaticFiles.SessionCart, count);
            }
            else
            {
                _unitofwork.ShoppingCart.DecrementCount(cart, 1);
            }
            _unitofwork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitofwork.ShoppingCart.GetFirstorDefault(u => u.Id == cartId);
            _unitofwork.ShoppingCart.Remove(cart);
            _unitofwork.Save();
            var count = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(StaticFiles.SessionCart, count);

            return RedirectToAction(nameof(Index));
        }




        private double GetPriceBasedOnQuantity(double quanity, double price, double price100)
        {
            if(quanity < 100)
            {
                return price;
            }
            else
            {
                return price100;
            }
              
        }
    }
}
