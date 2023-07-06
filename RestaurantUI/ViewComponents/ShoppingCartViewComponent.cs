using Microsoft.AspNetCore.Mvc;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Models.ViewModels;
using System.Security.Claims;

namespace RestaurantUI.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // current user
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            InfoCart info = new()
            {
                Count = 0,
                Amount = 0
            };
           
            
            if (claim != null)
            {
                List<ShoppingCart> list = (await _unitOfWork.ShoppingCartRepo.GetAll(
                 u=> u.UserId == claim.Value , includeProperties:"MenuItem" )).ToList();
                double total = 0;

                foreach (var item in list)
                {
                    total += (item.Count * item.MenuItem.Price);
                }
                info.Count = list.Count;
                info.Amount = total;

                return View(info);

            }
            else
            {
                return View(info);
            }

           
        }
    }
}
