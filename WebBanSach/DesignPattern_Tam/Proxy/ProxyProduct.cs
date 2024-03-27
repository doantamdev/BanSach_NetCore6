using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;

namespace WebBanSach.DesignPattern_Tam.Proxy
{
    public class ProxyProduct : ProxyUpdateProduct
    {
        private readonly Product _product;
        private readonly IUnitOfWork _unitOfWork;

        public ProxyProduct(Product product, IUnitOfWork unitOfWork)
        {
            _product = product;
            _unitOfWork = unitOfWork;
        }

        public string UpdateDatabase(IUnitOfWork unitOfWork, Product model)
        {
            string notAllow = "Tu ngu phan biet";
            if (_product.Title.Contains(notAllow))
            {
                return "Title không hợp lệ";
            }
            _unitOfWork.Product.Update(model);
            _unitOfWork.Save();
            return "Chỉnh sửa thành công";
        }
    }
}
