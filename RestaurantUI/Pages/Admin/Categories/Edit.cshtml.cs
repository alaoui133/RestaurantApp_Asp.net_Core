using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.Categories
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Category Categ { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _notify;

        public EditModel(IUnitOfWork unitOfWork, IToastNotification notify)
        {
            _unitOfWork = unitOfWork;
            _notify = notify;
        }
        public async Task OnGet(int id)
        {
            Categ = await _unitOfWork.CategoryRepo.GetById(c => c.Id == id);
            //Categ = _context.Category.FirstOrDefault(c => c.Id == id);
            //Categ = _context.Category.SingleOrDefault(c => c.Id == id);
            //Categ = _context.Category.Where(c => c.Id == id).FirstOrDefault();
            

        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepo.update(Categ);
                bool res = await _unitOfWork.Save();
                if (res)
                {
                    //TempData["msg"] = "Category Updated successfully";
                    //TempData["action"] = "edit";
                    _notify.AddSuccessToastMessage("Category Updated successfully");
                    return RedirectToPage("Index");

                }
                else
                {
                    _notify.AddErrorToastMessage("Category not Updated !!");
                    return Page();
                }
                
            }

            return Page();
        }
    }
}
