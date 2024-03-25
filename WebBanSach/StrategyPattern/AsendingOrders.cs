using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using WebBanSach.StrategyPattern;

namespace WebBanSach.Areas.StrateryPattern
{
    public class AsendingOrders : IStrategy
    {
        private readonly IUnitOfWork _unitOfWork;

        public AsendingOrders(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

		IEnumerable<Product> IStrategy.DoOrders()
		{
			return _unitOfWork.Product.GetAllByNameAsending(p => p.Title);
		}   
	}
}
