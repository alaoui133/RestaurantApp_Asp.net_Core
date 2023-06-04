using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace RestaurantUI.Pages.Admin.Categories
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Category Categ { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _notify;
        public DeleteModel(IUnitOfWork unitOfWork, IToastNotification notify)
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
            var Dcateg= await _unitOfWork.CategoryRepo.GetById(c =>c.Id==Categ.Id);
            if (Dcateg!=null)
            {
                _unitOfWork.CategoryRepo.remove(Dcateg);
                bool res = await _unitOfWork.Save();
                if (res) 
                {
                    //TempData["msg"] = "Category deleted successfully";
                    //TempData["action"] = "delete";
                    _notify.AddWarningToastMessage("Category deleted successfully");

                    return RedirectToPage("Index");
                }
                else
                {
                    _notify.AddErrorToastMessage("Category not deleted !!");
                    return Page();
                }
            }
            return Page();

        }
    }
}
