using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using NToastNotify;
using Restaurant.DAL;
using Restaurant.DAL.Interfaces;
using Restaurant.Models;
using Restaurant.Models.ViewModels;
using IoFile = System.IO.File;

namespace RestaurantUI.Pages.Admin.MenuItems
{
    public class AddOrEditModel : PageModel
    {
        [BindProperty]
        public MenuItemViewModel MenuItems { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _notify;
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FoodTypeList { get; set; }
        public readonly IMapper _mapper;
        private readonly IWebHostEnvironment _host;

        public AddOrEditModel(IUnitOfWork unitOfWork, IToastNotification notify,
            IMapper mapper, IWebHostEnvironment host)
        {
            _unitOfWork = unitOfWork;
            _notify = notify;
            _mapper = mapper;
            _host = host;
            MenuItems = new MenuItemViewModel();

        }
        public async Task OnGet(int? id)
        {
            if (id != null)
            {
                MenuItem mItem = await _unitOfWork.MenuItemRepo.GetById(c => c.Id == id);
                MenuItems = _mapper.Map<MenuItemViewModel>(mItem);// MenuItem to MenuItemViewModel
            }
            CategoryList = (await _unitOfWork.CategoryRepo.GetAll()).Select(
                c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

            FoodTypeList = (await _unitOfWork.FoodTypeRepo.GetAll()).Select(
               c => new SelectListItem()
               {
                   Text = c.FoodTypeName,
                   Value = c.FoodTypeId.ToString()
               });
        }

        public async Task<IActionResult> OnPost()
        {
            string webroot = _host.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var upload = Path.Combine(webroot, @"Images\MenuItems");
            // create operation
            if (MenuItems.Id == 0)
            {

                if (ModelState.IsValid)
                {
                    //MenuItem mItem = new MenuItem()
                    //{
                    //    Name = MenuItems.Name,
                    //    Description = MenuItems.Description,
                    //    ImageUrl = MenuItems.ImageUrl,
                    //    Price = MenuItems.Price,
                    //    FoodTypeId = MenuItems.FoodTypeId,
                    //    CategoryId = MenuItems.CategoryId,
                    //};
                    string fileName_img = Guid.NewGuid().ToString();

                    var extention = Path.GetExtension(files[0].FileName);
                    var completeFileName = Path.Combine(upload, fileName_img + extention);
                    // Upload File
                    using (var fileStream = new FileStream(completeFileName, FileMode.Create))
                    {
                        files[0].CopyToAsync(fileStream);
                    }
                    MenuItem mItem = _mapper.Map<MenuItem>(MenuItems);// transform ViewModel to model
                    mItem.ImageUrl = @"\Images\MenuItems\" + fileName_img + extention;
                    await _unitOfWork.MenuItemRepo.add(mItem);
                    bool res = await _unitOfWork.Save();
                    if (res)
                    {
                        _notify.AddSuccessToastMessage("Menu Item created successfully");

                        return RedirectToPage("Index");
                    }
                }
                else
                {
                    _notify.AddErrorToastMessage("Menu Item not created !!");

                }
            }
            else // id<>0 => Update Operation
            {
                MenuItem MenuItemUpdate = await _unitOfWork.MenuItemRepo.GetById(m=>m.Id==MenuItems.Id);
                string ImageUrl = MenuItemUpdate.ImageUrl;
                MenuItemUpdate = _mapper.Map<MenuItem>(MenuItems);
                if (files.Count>0)
                {
                    string fileName_img = Guid.NewGuid().ToString();

                    var extention = Path.GetExtension(files[0].FileName);
                    var completeFileName = Path.Combine(upload, fileName_img + extention);
                    
                    // delete MenuItem Image 
                   
                        var oldImagePath = Path.Combine(webroot, ImageUrl.TrimStart('\\'));
                        if (IoFile.Exists(oldImagePath))
                        {
                            IoFile.Delete(oldImagePath);

                        }

                    // Upload File
                    using (var fileStream = new FileStream(completeFileName, FileMode.Create))
                    {
                        files[0].CopyToAsync(fileStream);
                    }
                    MenuItemUpdate.ImageUrl= @"\Images\MenuItems\" + fileName_img + extention;

                }
                _unitOfWork.MenuItemRepo.update(MenuItemUpdate);
                if (await _unitOfWork.Save())
                {
                    _notify.AddSuccessToastMessage("Menu Item Updated successfully");

                    return RedirectToPage("Index");
                }
                else
                {
                    _notify.AddErrorToastMessage("Menu Item not Updated !!");
                }
                
            }
            

            return Page();
        }

        // ?Handler=Delete
        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id != null)
            {
                var mItem = await _unitOfWork.MenuItemRepo.GetById(c => c.Id == id);
                _unitOfWork.MenuItemRepo.remove(mItem);
                bool res = await _unitOfWork.Save();

                if (res)
                {
                    string webroot = _host.WebRootPath;
                    // delete MenuItem Image 
                    if (mItem.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(webroot, mItem.ImageUrl.TrimStart('\\'));
                        if (IoFile.Exists(oldImagePath))
                        {
                            IoFile.Delete(oldImagePath);

                        }
                    }

                    _notify.AddSuccessToastMessage("MenuItem deleted successfully");
                    return RedirectToPage("Index");
                }
                else
                {
                    _notify.AddErrorToastMessage("MenuItem not deleted !!!");
                }

            }
            else
            {
                return Page();
            }
            return Page();
        }


    }
}
