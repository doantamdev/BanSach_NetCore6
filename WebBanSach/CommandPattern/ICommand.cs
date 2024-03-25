using Microsoft.AspNetCore.Identity;

namespace WebBanSach.Areas.CommandPattern
{
    public interface ICommand
    {
        Task Execute();
        Task Undo();
    }
}
