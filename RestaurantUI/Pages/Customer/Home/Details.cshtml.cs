using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Models.ViewModels;
using Restaurant.Utilitiy;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RestaurantUI.Pages.Customer.Home
{
    [Authorize] // page available for connected user only 
    public class DetailsModel : PageModel
    {
        

        public IUnitOfWork _unitOfWork { get; }
        private readonly IToastNotification _notify;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCart { get; set; }
        public MenuItem MenuItem { get; set; }
        
        
        
        public DetailsModel(IUnitOfWork unitOfWork,IToastNotification notify)
        {
            _unitOfWork = unitOfWork;
             _notify = notify;
        }
        public async Task OnGet(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCart = new ShoppingCartViewModel()
            {
                MenuItemId = id,
                UserId     = userId,
                Count = 1
            };
            MenuItem = await _unitOfWork.MenuItemRepo.GetById
                             (m=>m.Id == id, "Category,FoodType");
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                ShoppingCart cartFromDb = await _unitOfWork.ShoppingCartRepo.GetById(
                  c=> c.MenuItemId == ShoppingCart.MenuItemId &&
                  c.UserId == ShoppingCart.UserId
                    );
                if (cartFromDb == null)
                {
                    ShoppingCart Cart = new ShoppingCart()
                    {
                        MenuItemId = ShoppingCart.MenuItemId,
                        Count = ShoppingCart.Count,
                        UserId = ShoppingCart.UserId
                    };
                    await _unitOfWork.ShoppingCartRepo.add(Cart);
                    if (await _unitOfWork.Save())
                    {
                        int count = (await _unitOfWork.ShoppingCartRepo.GetAll(
                            o=>o.UserId == Cart.UserId)).ToList().Count;
                        HttpContext.Session.SetInt32(ConstRoleDef.SessionCart, count);
                        _notify.AddSuccessToastMessage("MenuItem  Added to Cart Successfully");
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        _notify.AddAlertToastMessage("MenuItem Not Added to Cart !!");
                        return Page();
                    }
                }
                else
                {
                    _unitOfWork.ShoppingCartRepo.IncrementCount(cartFromDb, ShoppingCart.Count);
                }
               
            }
            return RedirectToPage("/Index");
        }
    }
}
