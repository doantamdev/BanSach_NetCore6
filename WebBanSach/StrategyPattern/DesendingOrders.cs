using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using WebBanSach.StrategyPattern;

namespace WebBanSach.Areas.StrateryPattern
{
	public class DesendingOrders : IStrategy
	{
		private readonly IUnitOfWork _unitOfWork;

		public DesendingOrders(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<Product> DoOrders()
		{
			return _unitOfWork.Product.GetAllByNameDesending(p => p.Title);
		}
	}
}
