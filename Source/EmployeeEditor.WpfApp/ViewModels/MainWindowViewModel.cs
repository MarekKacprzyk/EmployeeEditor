using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.WpfApp.Models.Csv;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
        private readonly MetroWindow _metroWindow;
        private readonly Func<EmployeeDto, EditEmployeeViewModel> _initEditEmployee;
        private EmployeeDto _selectedEmployee = null!;

        public MainWindowViewModel(
            CsvFileReader csvFileReader, 
            IWindowManager windowManager,
            MetroWindow metroWindow,
            Func<EmployeeDto, EditEmployeeViewModel> initEditEmployee)
        {
            _csvFileReader = csvFileReader;
            _windowManager = windowManager;
            _metroWindow = metroWindow;
            _initEditEmployee = initEditEmployee;
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
                NotifyOfPropertyChange(nameof(CanEdit));
            }
        }

        public bool CanEdit => SelectedEmployee != null;

        public async Task Edit()
        {
            try
            {
                var edit = _initEditEmployee.Invoke(SelectedEmployee);
                await _windowManager.ShowWindowAsync(edit);
            }
            catch (Exception exception)
            {
                await _metroWindow.ShowMessageAsync("Error", exception.Message);
            }
        }

        public async Task Load()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Pliki CSV (*.csv)|*.csv";
            ofd.Title = "Wybierz plik CSV";

            if (ofd.ShowDialog() ?? false)
            {
                var progressBar = await RunProgressBar();
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
                    await _metroWindow.ShowMessageAsync("Błąd", "Brak dostępu do pliku");
                }
                catch (Exception e)
                {
                    await _metroWindow.ShowMessageAsync("Błąd", e.Message);
                }
                finally
                {
                    await progressBar.CloseAsync();
                }
            }
        }

        private async Task<ProgressDialogController> RunProgressBar()
        {
            var controller = await _metroWindow.ShowProgressAsync("Tytuł okna", "Opis postępu", true);
            controller.SetIndeterminate();
            controller.SetTitle("Ładowanie");
            controller.SetMessage("Ładowanie danych z pliku");
            controller.Canceled += async (_, __) =>
            {
                await controller.CloseAsync();
            };
            return controller;
        }
    }
}
