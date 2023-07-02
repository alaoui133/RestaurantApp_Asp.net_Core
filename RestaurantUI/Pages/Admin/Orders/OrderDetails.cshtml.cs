using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Models.ViewModels;
using Restaurant.Utilitiy;

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
        public async Task<IActionResult> OnPostOrderComplete(int OrderId)
        {
            _unitOfWork.OrderRepo.UpdateStatus(OrderId, ConstRoleDef.StatusCompleted);
            await _unitOfWork.Save();
            return RedirectToPage("OrderList");
        }

        public async Task<IActionResult> OnPostOrderCancel(int OrderId)
        {
            _unitOfWork.OrderRepo.UpdateStatus(OrderId,ConstRoleDef.StatusCancelled);
            await _unitOfWork.Save();
            return RedirectToPage("OrderList");
        } 
        public async Task<IActionResult> OnPostRefundOrder(int OrderId)
        {
            _unitOfWork.OrderRepo.UpdateStatus(OrderId,ConstRoleDef.StatusRefunded);
            await _unitOfWork.Save();
            return RedirectToPage("OrderList");
        }
    }
}
