using Microsoft.AspNetCore.Identity;

namespace WebBanSach.Areas.CommandPattern
{
    public class Invoker
    {
        private List<ICommand> _commands;

        public Invoker(List<ICommand> commands) { 
            _commands = commands;
        }

        public void AddCommand(ICommand Command)
        {
            _commands.Add(Command);
        }

        public async Task ExecuteCommand()
        {
            foreach (var command in _commands)
            {
                await command.Execute();
            }

            _commands.Clear();
        }

        public async Task UndoLastCommand() {
            if (_commands.Count > 0)
            {
                var last = _commands.Last();
                await last.Undo();

                _commands.Remove(last);
            }
        }
    }
}
