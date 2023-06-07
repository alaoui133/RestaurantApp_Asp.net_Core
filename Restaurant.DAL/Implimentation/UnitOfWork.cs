using Restaurant.DAL.Interfaces;

namespace Restaurant.DAL.Implimentation
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository CategoryRepo { get; private set; }

        public IFoodTypeRepository FoodTypeRepo { get; private set; }

        public IMenuItemRepository MenuItemRepo { get; private set; }

        public IShoppingCartRepository ShoppingCartRepo { get; private set; }

        public IOrderRepository OrderRepo { get; private set; }

        public IOrderDetailsRepository OrderDetailsRepo { get; private set; }

        private readonly RestaurantDBContext _db;
        public UnitOfWork(RestaurantDBContext db)
        {
            _db = db;
            CategoryRepo = new CategoryRepository(_db);
            FoodTypeRepo = new FoodTypeRepository(_db);
            MenuItemRepo = new MenuItemRepository(_db);
            ShoppingCartRepo = new ShoppingCartRepository(_db);
            OrderRepo = new OrderRepository(_db);
            OrderDetailsRepo = new OrderDetailsRepository(_db);

        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
