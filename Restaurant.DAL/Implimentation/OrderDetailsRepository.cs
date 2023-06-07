using Restaurant.DAL.Interfaces;
using Restaurant.Models;

namespace Restaurant.DAL.Implimentation
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        public RestaurantDBContext _db => _context as RestaurantDBContext;
        public OrderDetailsRepository(RestaurantDBContext context) : base(context)
        {

        }
     
        public void update(OrderDetails orderDetails)
        {
            _db.OrderDetails.Update(orderDetails);
        }
    }
}
