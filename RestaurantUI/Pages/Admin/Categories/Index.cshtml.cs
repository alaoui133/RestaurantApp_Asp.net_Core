using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {   private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<Category> Categories { get; set; }
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public async Task OnGet()
        {
            Categories = await _unitOfWork.CategoryRepo.GetAll();
        }
    }
}
