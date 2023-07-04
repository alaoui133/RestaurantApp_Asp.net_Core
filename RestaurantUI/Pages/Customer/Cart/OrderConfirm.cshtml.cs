using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Utilitiy;
using Stripe.Checkout;

namespace RestaurantUI.Pages.Customer.Cart
{
    public class OrderConfirmModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public int OrderId { get; set; }

        public OrderConfirmModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task OnGet(int id)
        {
            Order order = await _unitOfWork.OrderRepo.GetById(o=> o.Id == id);
            if (order.sessionId != null) 
            {
                var service = new SessionService();
                Session session = service.Get(order.sessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    order.Status = ConstRoleDef.StatusSubmitted;
                    order.PaymentIntentId = session.PaymentIntentId;

                    // remove Shopping Cart
                    List<ShoppingCart>list = (await _unitOfWork.ShoppingCartRepo.GetAll(
                        filter:u=>u.UserId == order.UserId)).ToList();
                    _unitOfWork.ShoppingCartRepo.removeRange(list);
                    await _unitOfWork.Save();
                    OrderId = id;

                }
            }
        }
    }
}
