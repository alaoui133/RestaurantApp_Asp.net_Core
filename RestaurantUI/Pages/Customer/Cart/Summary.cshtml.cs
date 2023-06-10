using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL.Implimentation;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Utilitiy;
using Stripe.Checkout;
using System.Security.Claims;

namespace RestaurantUI.Pages.Customer.Cart
{
    [Authorize]
    public class SummaryModel : PageModel
    {
        public readonly IUnitOfWork _UnitOfWork;

        [BindProperty]
        public Order Order { get; set; }
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        private readonly IToastNotification _notify;
        public bool status = false;

        public SummaryModel(IUnitOfWork unitOfWork, IToastNotification notify)
        {
            _UnitOfWork = unitOfWork;
            Order = new Order();
            _notify = notify;
        }
        public async Task OnGet()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (UserId != null)
            {
                ShoppingCartList = await _UnitOfWork.ShoppingCartRepo.GetAll(

                    filter: u => u.UserId == UserId,
                    includeProperties: "ApplicationUser,MenuItem"
                    );

                foreach (var cart in ShoppingCartList)
                {
                    Order.OrderTotal += (cart.MenuItem.Price * cart.Count);
                }

                ApplicationUser AppUser = ShoppingCartList.First().ApplicationUser;
                Order.PickUpName = AppUser.FirstName + " " + AppUser.LastName;
                Order.PhoneNumber = AppUser.PhoneNumber;
                Order.OrderDate = DateTime.Now;
                Order.PickUpTime = DateTime.Now;
            }

        }

        public async Task<IActionResult> OnPost()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (UserId != null)
            {
                ShoppingCartList = await _UnitOfWork.ShoppingCartRepo.GetAll(

                    filter: u => u.UserId == UserId,
                    includeProperties: "ApplicationUser,MenuItem"
                    );

                foreach (var cart in ShoppingCartList)
                {
                    Order.OrderTotal += (cart.MenuItem.Price * cart.Count);
                }

                // Add Order

                Order.Status = ConstRoleDef.StatusPending;
                Order.UserId = UserId;
                Order.OrderDate = DateTime.Now;
                Order.PickUpTime = Convert.ToDateTime(
                  Order.PickUpDate.ToShortDateString() + " " + Order.PickUpTime.ToShortTimeString());

                await _UnitOfWork.OrderRepo.add(Order);
                await _UnitOfWork.Save();

                // Add Order Details

                foreach (ShoppingCart item in ShoppingCartList)
                {
                    OrderDetails orderDetails = new OrderDetails()
                    {
                        MenuItemId = item.MenuItemId,
                        OrderId = Order.Id,
                        Name = item.MenuItem.Name,
                        Price = item.MenuItem.Price,
                        Count = item.Count
                    };

                    await _UnitOfWork.OrderDetailsRepo.add(orderDetails);

                }
                _UnitOfWork.ShoppingCartRepo.removeRange(ShoppingCartList);
                await _UnitOfWork.Save();
                _notify.AddSuccessToastMessage("Order Added Successfully");


                //var domain = "http://localhost:4242";
                string strProtocol = HttpContext.Request.IsHttps ? "https://" : "http://";
                string host = HttpContext.Request.Host.Value;
                var domain = strProtocol + host;
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },

                    Mode = "payment",
                    SuccessUrl = domain + $"/Customer/Cart/OrderConfirm?id={Order.Id}",
                    CancelUrl = domain + "/Customer/Cart/Index",
                };

                foreach (var item in ShoppingCartList)
                {
                    var sessionLineItems = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = ((long?)(item.MenuItem.Price * 100)),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.MenuItem.Name,
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItems);
                }
                var service = new SessionService();
                Session session = service.Create(options);

                Response.Headers.Add("Location", session.Url);
                Order.sessionId = session.Id;
                await _UnitOfWork.Save();
                return new StatusCodeResult(303);




            }

            return Page();

        }
    }
}
