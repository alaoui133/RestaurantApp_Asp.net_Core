using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.FoodTypes
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public FoodType foodType { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _notify;

        public EditModel(IUnitOfWork unitOfWork, IToastNotification notify)
        {
            _unitOfWork = unitOfWork;
            _notify = notify;
        }
        public async Task OnGet(int id)
        {
            foodType = await _unitOfWork.FoodTypeRepo.GetById(f=>f.FoodTypeId==id);
           
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.FoodTypeRepo.update(foodType);
                bool res = await _unitOfWork.Save();
                if (res)
                {
                    //TempData["msg"] = "Food Type Updated successfully";
                    //TempData["action"] = "edit";
                    _notify.AddSuccessToastMessage("Food Type Updated successfully");
                    return RedirectToPage("Index");

                }
                else
                {
                    _notify.AddErrorToastMessage("FoodType not Updated !!");
                    return Page();
                }
                
            }

            return Page();
        }
    }
}
