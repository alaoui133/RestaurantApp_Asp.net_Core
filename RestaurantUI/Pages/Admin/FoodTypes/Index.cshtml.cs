using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.FoodTypes
{
    public class IndexModel : PageModel
    {   private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<FoodType> FoodTypes { get; set; }
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public async Task OnGet()
        {
            FoodTypes = await _unitOfWork.FoodTypeRepo.GetAll();
        }

       
    }
}
