namespace Restaurant.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepo { get; }
        IFoodTypeRepository FoodTypeRepo { get; }
        IMenuItemRepository MenuItemRepo { get; }
        IShoppingCartRepository ShoppingCartRepo { get; }
        IOrderRepository OrderRepo { get; }
        IOrderDetailsRepository OrderDetailsRepo { get; }
        Task<bool> Save();
    }
}
