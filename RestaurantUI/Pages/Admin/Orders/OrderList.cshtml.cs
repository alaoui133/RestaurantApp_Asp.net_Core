using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

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

        public async Task<IActionResult> OnGetList()
        {
            orderList = await _unitOfWork.OrderRepo.GetAll(includeProperties: "ApplicationUser");
            return new JsonResult(new {data = orderList});
        }
    }
}
