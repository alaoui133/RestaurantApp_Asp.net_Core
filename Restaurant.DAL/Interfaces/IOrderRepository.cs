using Restaurant.Models;

namespace Restaurant.DAL.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void update(Order order); 
        



    }
}
