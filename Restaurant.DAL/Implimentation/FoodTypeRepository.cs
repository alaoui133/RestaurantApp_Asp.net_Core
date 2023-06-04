using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace Restaurant.DAL.Implimentation
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {
        public RestaurantDBContext _db => _context as RestaurantDBContext;
        public FoodTypeRepository(RestaurantDBContext context) : base(context)
        {

        }
      

        public void update(FoodType foodType)
        {
            FoodType ftype = _db.FoodTypes.Find(foodType.FoodTypeId);
            if (ftype != null)
            {
                ftype.FoodTypeName = foodType.FoodTypeName;
               
            }
        }

        
    }
}
