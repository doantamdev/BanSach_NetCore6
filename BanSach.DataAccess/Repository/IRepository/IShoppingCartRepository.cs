 using BanSach.Models;

namespace BanSach.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
       int IncreamentCount(ShoppingCart shoppingCart,int count);
       int DecreamentCount(ShoppingCart shoppingCart, int count);
    }
}
