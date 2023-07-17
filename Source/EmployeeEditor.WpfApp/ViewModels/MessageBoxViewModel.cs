using Caliburn.Micro;
using System.Threading.Tasks;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public class MessageBoxViewModel : Screen
    {
        public string Message { get; }

        public MessageBoxViewModel(string message)
        {
            Message = message;
        }

        public async Task Ok()
        {
            await TryCloseAsync(true);
        }
    }
}
