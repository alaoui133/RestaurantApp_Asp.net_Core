using Restaurant.Models;

namespace Restaurant.DAL.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        void update(MenuItem menuItem); 
        


    }
}
