namespace WebBanSach.Areas.IteratorPattern
{
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }
}
