using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Models.ViewModels;

namespace RestaurantUI.Pages.Admin.Orders
{
    public class OrderDetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailsViewModel OrderDetailsVm { get; set; }

        public OrderDetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public async Task OnGet(int id)
        {
            OrderDetailsVm = new()
            {
                order = await _unitOfWork.OrderRepo.GetById(
                    o => o.Id == id, includeProperties: "ApplicationUser"),
                orderDetails = await _unitOfWork.OrderDetailsRepo.GetAll(o => o.Id == id)
            };
              
           
          
        }
    }
}
