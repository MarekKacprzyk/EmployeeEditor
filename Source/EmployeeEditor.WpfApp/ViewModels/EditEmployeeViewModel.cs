using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public sealed class EditEmployeeViewModel : Screen
    {
        private readonly EmployeeDto _employee;

        public EditEmployeeViewModel(EmployeeDto employee)
        {
            _employee = employee;
            DisplayName = "Edit employee panel";
        }

        public string Name
        {
            get => _employee.Name;
            set => _employee.Name = value;
        }

        public string Surename
        {
            get => _employee.Surename;
            set => _employee.Surename = value;
        }

        public string PhoneNumber
        {
            get => _employee.PhoneNumber;
            set => _employee.PhoneNumber = value;
        }

        public string Email
        {
            get => _employee.Email;
            set => _employee.Email = value;
        }
    }
}
