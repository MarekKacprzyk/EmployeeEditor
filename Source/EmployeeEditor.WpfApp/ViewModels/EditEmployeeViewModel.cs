using System;
using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.WpfApp.Models.Validators;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public sealed class EditEmployeeViewModel : Screen, IDataErrorInfo
    {
        private readonly EmployeeDto _employee;
        private readonly EmployeeValidator _validator;
        private readonly IWindowManager _windowManager;
        private readonly Func<string, MessageBoxViewModel> _messageBoxFactory;

        public EditEmployeeViewModel(EmployeeDto employee, 
            EmployeeValidator validator, 
            IWindowManager windowManager,
            Func<string, MessageBoxViewModel> messageBoxFactory)
        {
            _employee = employee;
            _validator = validator;
            _windowManager = windowManager;
            _messageBoxFactory = messageBoxFactory;
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

        public async Task Exit()
        {
            await TryCloseAsync();
        }

        public override async Task TryCloseAsync(bool? dialogResult = null)
        {
            var result = await _validator.ValidateAsync(_employee);

            var hasAnyError = result.Errors?.Any() ?? false;
            if (hasAnyError)
            {
                var messageBox = _messageBoxFactory.Invoke($"Nieprawidłowe dane:\n\n{result.Errors[0].ErrorMessage}");

                await _windowManager.ShowDialogAsync(messageBox);
                //_metroWindow.ShowMessageAsync("Error", );
            }
            await base.TryCloseAsync(hasAnyError);
        }
    }
}
