using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Models.ViewModels;
using Restaurant.Utilitiy;

namespace RestaurantUI.Pages.Admin.Orders
{
    public class OrderManagerModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public List<OrderDetailsViewModel> OrderDetailsViewModelList { get; set; }
        public OrderManagerModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            OrderDetailsViewModelList = new List<OrderDetailsViewModel>();
        }
        public async Task OnGet()
        {
            List<Order> order = (await _unitOfWork.OrderRepo.GetAll(
                filter: o => o.Status == ConstRoleDef.StatusSubmitted
                || o.Status == ConstRoleDef.StatusInProcess)).ToList();
            foreach (Order ord in order)
            {
                OrderDetailsViewModel ordVM = new()
                {
                    order = ord,
                    orderDetails = await _unitOfWork.OrderDetailsRepo.GetAll
                    (
                        o => o.OrderId == ord.Id
                    )
                };
                OrderDetailsViewModelList.Add(ordVM);
            }
        }
    }
}

