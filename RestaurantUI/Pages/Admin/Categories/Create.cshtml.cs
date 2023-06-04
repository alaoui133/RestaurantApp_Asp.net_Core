using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.Categories
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Category Categ { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _notify;
        public CreateModel(IUnitOfWork unitOfWork, IToastNotification notify)
        {
            _unitOfWork = unitOfWork;
            _notify = notify;
        }
        public void OnGet()
        {
            Categ = new Category();

        }

        public async Task<IActionResult> OnPost()
        {
            if (Categ.DisplayOrder.ToString() == Categ.Name)
            {
                ModelState.AddModelError("CustumError", "displayOrder should be <>");
            }
            bool isDigitExist = !string.IsNullOrEmpty(Categ.Name) && Categ.Name.Any(c => char.IsDigit(c));

            if (isDigitExist)
            {
                ModelState.AddModelError("ErrorDigit", "Name should not contain a digit");
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.CategoryRepo.add(Categ);
                bool res = await _unitOfWork.Save();
                if (res)
                {
                    _notify.AddSuccessToastMessage("Category Created successfully");
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
