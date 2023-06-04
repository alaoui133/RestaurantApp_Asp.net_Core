using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace Restaurant.DAL.Implimentation
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        public RestaurantDBContext _db => _context as RestaurantDBContext;
        public MenuItemRepository(RestaurantDBContext context) : base(context)
        {

        }
      

        public void update(MenuItem menuItem)
        {
            MenuItem mItem = _db.MenuItem.Find(menuItem.Id);
            if (mItem != null)
            {
                mItem.Name = menuItem.Name;
                mItem.Description = menuItem.Description;
                mItem.Price = menuItem.Price;
                mItem.CategoryId = menuItem.CategoryId;
                mItem.FoodTypeId = menuItem.FoodTypeId;
                if (menuItem.ImageUrl != null)
                {
                    mItem.ImageUrl = menuItem.ImageUrl;

                }
            }
        }

      
    }
}
