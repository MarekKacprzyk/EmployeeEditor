using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.WpfApp.Models.Csv;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        private readonly Func<string, MessageBoxViewModel> _initMessageBox;
        private readonly Func<EmployeeDto, EditEmployeeViewModel> _initEditEmployee;
        private EmployeeDto _selectedEmployee = null!;

        public MainWindowViewModel(
            CsvFileReader csvFileReader, 
            IWindowManager windowManager,
            MetroWindow metroWindow,
            Func<string, MessageBoxViewModel> initMessageBox,
            Func<EmployeeDto, EditEmployeeViewModel> initEditEmployee)
        {
            _csvFileReader = csvFileReader;
            _windowManager = windowManager;
            _metroWindow = metroWindow;
            _initMessageBox = initMessageBox;
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
            var settings = new Dictionary<string, object>()
            {
                //d:DesignHeight = "600" d: DesignWidth = "400"
                { "Width", 60},
                { "Height", 160}
            };
            var edit = _initEditEmployee.Invoke(SelectedEmployee);
            await _windowManager.ShowWindowAsync(edit);
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
                    var progressBar = await RunProgressBar();
                    //await Task.Delay(100);

                    LoadEmployeeFromCsvFile(ofd.FileName);

                    //await Task.Delay(100);
                    await progressBar.CloseAsync();
                }
                catch (IOException e)
                {
                    await _metroWindow.ShowMessageAsync("Błąd", "Brak dostępu do pliku");
                }
                catch (Exception e)
                {
                    await _metroWindow.ShowMessageAsync("Błąd", "Nieznany błąd");
                }
            }
        }

        private async Task<ProgressDialogController> RunProgressBar()
        {
            var controller = await _metroWindow.ShowProgressAsync("Tytuł okna", "Opis postępu", true);
            controller.SetIndeterminate();
            controller.SetTitle("Ładowanie");
            controller.SetMessage("Ładowanie danych z pliku");
            return controller;
        }

        private void LoadEmployeeFromCsvFile(string filePath)
        {
            var employees = _csvFileReader.ReadAllEmployee(filePath);

            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }
    }
}
