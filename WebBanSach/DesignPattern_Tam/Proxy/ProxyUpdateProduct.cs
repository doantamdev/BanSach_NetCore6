using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;

namespace WebBanSach.DesignPattern_Tam.Proxy
{
    public interface ProxyUpdateProduct
    {
        string UpdateDatabase(IUnitOfWork unitOfWork, Product model);
    }

}
