namespace Restaurant.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepo { get; }
        IFoodTypeRepository FoodTypeRepo { get; }
        IMenuItemRepository MenuItemRepo { get; }
        IShoppingCartRepository ShoppingCartRepo { get; }
        Task<bool> Save();
    }
}
