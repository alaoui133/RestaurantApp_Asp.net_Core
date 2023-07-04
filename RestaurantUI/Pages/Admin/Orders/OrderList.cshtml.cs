using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Utilitiy;

namespace RestaurantUI.Pages.Admin.Orders
{
    public class OrderListModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<Order> orderList { get; set; }
        public OrderListModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetList(string? status = null)
        {
            orderList = await _unitOfWork.OrderRepo.GetAll(includeProperties: "ApplicationUser");
            switch (status)
            {
                case "cancelled":
                    orderList = orderList.Where(o => o.Status == ConstRoleDef.StatusCancelled
                    || o.Status == ConstRoleDef.StatusRejected);
                    break;

                case "completed":
                    orderList = orderList.Where(o => o.Status == ConstRoleDef.StatusCompleted);
                    break;

                case "inprocess":
                    orderList = orderList.Where(o => o.Status == ConstRoleDef.StatusInProcess);
                    break;
                case "ready":
                    orderList = orderList.Where(o => o.Status == ConstRoleDef.StatusReady);
                    break;
                default:
                    orderList = orderList;
                    break;
            }
            return new JsonResult(new {data = orderList});
        }
    }
}
