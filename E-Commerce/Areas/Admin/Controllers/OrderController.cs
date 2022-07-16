using E_Commerce.DataAccess.Repository.IRepository;
using E_Commerce.Models;
using E_Commerce.Models.ViewModels;
using E_Commerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Areas.Admin.Controllers
{   [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }    

        public OrderController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderType = _unitofwork.OrderType.GetFirstorDefault(u=>u.Id==orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitofwork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product"),
            };
            return View(OrderVM);
        }
        [HttpPost]
        public IActionResult UpdateOrderDetail()
        {
            var orderTypeFromDb = _unitofwork.OrderType.GetFirstorDefault(u => u.Id == OrderVM.OrderType.Id);
            orderTypeFromDb.Name = OrderVM.OrderType.Name;
            orderTypeFromDb.PhoneNumber = OrderVM.OrderType.PhoneNumber;
            orderTypeFromDb.Address = OrderVM.OrderType.Address;
            orderTypeFromDb.City = OrderVM.OrderType.City;
            orderTypeFromDb.PostalCode = OrderVM.OrderType.PostalCode;
            orderTypeFromDb.Region = OrderVM.OrderType.Region;

            if (OrderVM.OrderType.TrackingNumber != null)
            {
                orderTypeFromDb.TrackingNumber = OrderVM.OrderType.TrackingNumber;
            }

            _unitofwork.OrderType.Update(orderTypeFromDb);
            _unitofwork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = orderTypeFromDb.Id });

        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderType> orderTypes;

            if (User.IsInRole(StaticFiles.Role_Admin))
            {
                orderTypes = _unitofwork.OrderType.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderTypes = _unitofwork.OrderType.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            }
            

            switch (status)
            {
                case "pending":
                    orderTypes = orderTypes.Where(u => u.PaymentStatus == StaticFiles.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderTypes = orderTypes.Where(u => u.OrderStatus == StaticFiles.StatusInProcess);
                    break;
                case "completed":
                    orderTypes = orderTypes.Where(u => u.OrderStatus == StaticFiles.StatusShipped);
                    break;
                case "approved":
                    orderTypes = orderTypes.Where(u => u.OrderStatus == StaticFiles.StatusApproved);
                    break;
                default:
                    break;
            }


            return Json(new { data = orderTypes });
        }
        #endregion
    }
}
