using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.WpfApp.Models.Csv;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public sealed class MainWindowViewModel : Conductor<object>
    {
        private readonly CsvFileReader _csvFileReader;
        private readonly IWindowManager _windowManager;
        private readonly Func<string, MessageBoxViewModel> _initMessageBox;
        private EmployeeDto _selectedEmployee = null!;

        public MainWindowViewModel(
            CsvFileReader csvFileReader, 
            IWindowManager windowManager, 
            Func<string, MessageBoxViewModel> initMessageBox)
        {
            _csvFileReader = csvFileReader;
            _windowManager = windowManager;
            _initMessageBox = initMessageBox;
            Employees = new ObservableCollection<EmployeeDto>();
            DisplayName = "Employee Editor v1.0";
        }

        public ObservableCollection<EmployeeDto> Employees { get; }

        public EmployeeDto SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (Equals(value, _selectedEmployee)) return;
                _selectedEmployee = value;
                NotifyOfPropertyChange();
            }
        }

        public async Task Load()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Pliki CSV (*.csv)|*.csv";
            ofd.Title = "Wybierz plik CSV";


            if (ofd.ShowDialog() ?? false)
            {
                try
                {
                    var employees = _csvFileReader.ReadAllEmployee(ofd.FileName);

                    Employees.Clear();
                    foreach (var employee in employees)
                    {
                        Employees.Add(employee);
                    }
                }
                catch (IOException e)
                {
                    var messageBox = _initMessageBox.Invoke(e.Message);
                    var result = await _windowManager.ShowDialogAsync(messageBox);

                }
            }
        }
    }
}
