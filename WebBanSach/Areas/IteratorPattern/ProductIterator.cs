using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;

namespace WebBanSach.Areas.IteratorPattern
{
    public class ProductIterator : IIterator<Product>
    {
        private List<Product> _products;
        private int _index = 0;

        public ProductIterator(IUnitOfWork unitOfWork)
        {
            _products = unitOfWork.Product.GetAll(includeProperties: "Category,CoverType").ToList();
        }

        public bool HasNext()
        {
            return _index < _products.Count;
        }

        public Product Next()
        {
            if (HasNext())
            {
                Product product = _products[_index];
                _index++;
                return product;
            }
            return null;
        }
    }
}
