using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace Restaurant.DAL.Implimentation
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public RestaurantDBContext _db => _context as RestaurantDBContext;
        public ShoppingCartRepository(RestaurantDBContext context) : base(context)
        {

        }

        public int IncrementCount(ShoppingCart cart, int count)
        {
            cart.Count+=count;
            _db.SaveChanges();
            return cart.Count;
        }

        public int DecrementCount(ShoppingCart cart, int count)
        {
            cart.Count-=count;
            _db.SaveChanges();
            return cart.Count;
        }
    }
}
