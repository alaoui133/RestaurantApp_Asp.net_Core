using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.FoodTypes
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public FoodType foodType { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _notify;
        public DeleteModel(IUnitOfWork unitOfWork, IToastNotification notify)
        {
            _unitOfWork = unitOfWork;
            _notify = notify;
        }
        public async Task OnGet(int id)
        {
            foodType = await _unitOfWork.FoodTypeRepo.GetById(f=>f.FoodTypeId ==id);
            

        }

        public async Task<IActionResult> OnPost()
        {
            var Dftype= await _unitOfWork.FoodTypeRepo.GetById(f => f.FoodTypeId == foodType.FoodTypeId);
           
            if (Dftype != null)
            {
                _unitOfWork.FoodTypeRepo.remove(Dftype);
                bool res = await _unitOfWork.Save();
                if (res) 
                {
                    //TempData["msg"] = "Food Type deleted successfully";
                    //TempData["action"] = "delete";
                    _notify.AddWarningToastMessage("Food Type deleted successfully");

                    return RedirectToPage("Index");
                }
                else
                {
                    _notify.AddErrorToastMessage("Food Type not deleted !!");
                    return Page();
                }
            }
            return Page();

        }
    }
}
