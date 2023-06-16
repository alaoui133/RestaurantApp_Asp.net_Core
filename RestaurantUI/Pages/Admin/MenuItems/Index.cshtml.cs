using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.MenuItems
{
    public class IndexModel : PageModel
    {   private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<MenuItem> menuItems { get; set; }
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public async Task OnGet()
        {
            menuItems = await _unitOfWork.MenuItemRepo.GetAll();
        }

        public async Task<IActionResult> OnGetList()
        {
            menuItems = await _unitOfWork.MenuItemRepo.GetAll(null,"Category,FoodType");
            return new JsonResult(new {data = menuItems });
           
        }
    }
}
