using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace Restaurant.DAL.Implimentation
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public RestaurantDBContext _db => _context as RestaurantDBContext;
        public OrderRepository(RestaurantDBContext context) : base(context)
        {

        }

        public void update(Order order)
        {
         _db.Order.Update(order);
        }
    }
}
