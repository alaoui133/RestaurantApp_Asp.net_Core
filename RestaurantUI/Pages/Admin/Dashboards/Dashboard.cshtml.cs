using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Utilitiy;

namespace RestaurantUI.Pages.Admin.Dashboard
{
    public class DashboardModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public List<OrderDetails> OrderDetails { get; set; }
        public double TotalM { get; set; }
        public double TotalY { get; set; }
        public int TotalAprovePay { get; set; }
        public int TotalReady { get; set; }


        public DashboardModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task OnGet()
        {
            TotalAprovePay = 0;
            TotalReady= 0;
            OrderDetails = (List<OrderDetails>)await _unitOfWork.OrderDetailsRepo.GetAll(includeProperties:"Order");
           
            foreach (var item in OrderDetails)
            {
                DateTime dtOrder = new DateTime();
                dtOrder = item.Order.OrderDate;
                if (dtOrder.Month == DateTime.Now.Month && 
                    item.Order.Status== ConstRoleDef.StatusCompleted)
                {
                    TotalM += item.Price * item.Count;
                }

                if (dtOrder.Year == DateTime.Now.Year && 
                    item.Order.Status == ConstRoleDef.StatusCompleted)
                {
                    TotalY += item.Price * item.Count;
                }

                if (item.Order.Status == ConstRoleDef.StatusSubmitted)
                {
                    TotalAprovePay++;
                }

                if (item.Order.Status == ConstRoleDef.StatusReady)
                {
                    TotalReady++;
                }
                
                
            }
        }
    }
}
