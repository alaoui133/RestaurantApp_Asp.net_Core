using Restaurant.Models;

namespace Restaurant.DAL.Interfaces
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
        void update(OrderDetails orderDetails); 
        
    }
}
