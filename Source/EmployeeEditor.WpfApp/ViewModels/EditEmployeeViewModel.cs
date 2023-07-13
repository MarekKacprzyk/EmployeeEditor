using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.WpfApp.Models.Validators;
using System.ComponentModel;
using System.Linq;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public sealed class EditEmployeeViewModel : Screen, IDataErrorInfo
    {
        private readonly EmployeeDto _employee;
        private readonly EmployeeValidator _validator;

        public EditEmployeeViewModel(EmployeeDto employee, EmployeeValidator validator)
        {
            _employee = employee;
            _validator = validator;
            DisplayName = "Edit employee panel";
        }

        public string Name
        {
            get => _employee.Name;
            set
            {
                _employee.Name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Surename
        {
            get => _employee.Surename;
            set
            {
                _employee.Surename = value;
                NotifyOfPropertyChange();
            }
        }

        public string PhoneNumber
        {
            get => _employee.PhoneNumber;
            set
            {
                _employee.PhoneNumber = value;
                NotifyOfPropertyChange();
            }
        }

        public string Email
        {
            get => _employee.Email;
            set
            {
                _employee.Email = value;
                NotifyOfPropertyChange();
            }
        }

        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                var result = _validator.Validate(_employee);
                
                if (result.IsValid) return null;

                var selectedErro = result.Errors.FirstOrDefault(e => e.PropertyName == columnName);
                return selectedErro?.ErrorMessage;
            }
        }
    }
}
