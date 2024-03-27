using BanSach.Models;

namespace WebBanSach.DesignPattern_Tam.Iterator
{
    public interface ITerator
    {
        Product First();
        Product Next();
        bool IsDone { get; }
        Product CurrentItem { get; }
    }
}
