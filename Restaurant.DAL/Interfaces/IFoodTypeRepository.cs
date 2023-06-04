using Restaurant.Models;

namespace Restaurant.DAL.Interfaces
{
    public interface IFoodTypeRepository : IRepository<FoodType>
    {
        void update(FoodType foodType); 
        


    }
}
