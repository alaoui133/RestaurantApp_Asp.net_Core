using Restaurant.Models;

namespace Restaurant.DAL.Interfaces
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void update(Category category); 
        


    }
}
