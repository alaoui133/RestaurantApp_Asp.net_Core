using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.FoodTypes
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public FoodType foodType { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _notify;
        public CreateModel(IUnitOfWork unitOfWork, IToastNotification notify)
        {
            _unitOfWork = unitOfWork;
            _notify = notify;
        }
        public void OnGet()
        {
            foodType = new FoodType();

        }

        public async Task<IActionResult> OnPost()
        {
           
            bool isDigitExist = !string.IsNullOrEmpty(foodType.FoodTypeName) && foodType.FoodTypeName.Any(ft => char.IsDigit(ft));

            if (isDigitExist)
            {
                ModelState.AddModelError("ErrorDigit", "Name should not contain a digit");
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.FoodTypeRepo.add(foodType);
                bool res = await _unitOfWork.Save();
                if (res)
                {
                    _notify.AddSuccessToastMessage("Food Type Created successfully");
                    return RedirectToPage("index");
                    //TempData["msg"] = "Category Created successfully";
                    //TempData["action"] = "create";
                }
                else
                {
                    _notify.AddErrorToastMessage("Category not Created !!");
                    return Page();

                }
                


            }

            return Page();
        }
    }
}
