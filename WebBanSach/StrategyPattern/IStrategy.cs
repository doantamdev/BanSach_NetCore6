using BanSach.Models;

namespace WebBanSach.StrategyPattern
{
    public interface IStrategy
    {
        IEnumerable<Product> DoOrders();
    }
}
