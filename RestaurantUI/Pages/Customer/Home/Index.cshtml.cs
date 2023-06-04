using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        public IUnitOfWork _unitOfWork { get; }
        public IEnumerable<MenuItem> MenuItemsList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnGet()
        {
            MenuItemsList = await _unitOfWork.MenuItemRepo.GetAll(filter: null, includeProperties: "Category,FoodType");
            var idsCategs = MenuItemsList.Select(x => x.CategoryId).Distinct().ToList();

            CategoryList = (await _unitOfWork.CategoryRepo.GetAll(OrderBy:cat=>cat.OrderBy(c=>c.DisplayOrder)))
                .Where(c => idsCategs.Contains(c.Id));
               
        }
    }
}

