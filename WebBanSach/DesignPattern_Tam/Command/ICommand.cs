namespace WebBanSach.DesignPattern_Tam.Command
{
    public interface ICommand<T> : IUndoItem
    {
        void Execute();
    }
}
