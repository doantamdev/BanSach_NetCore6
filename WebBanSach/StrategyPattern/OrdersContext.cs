using BanSach.Models;

namespace WebBanSach.StrategyPattern
{
    public class OrdersContext
    {
        private IStrategy _strategy;

        public OrdersContext(IStrategy strategy) {
            _strategy = strategy;
        }

        public IEnumerable<Product> ExcuteOrders()
        {
			IEnumerable<Product> ProductList = _strategy.DoOrders();

            return ProductList;
        }
    }
}
