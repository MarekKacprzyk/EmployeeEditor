﻿using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.Domain.Interfaces;
using EmployeeEditor.WpfApp.Models.Csv;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public sealed class MainWindowViewModel : Conductor<object>
    {
        private readonly CsvFileReader _csvFileReader;
        private readonly IWindowManager _windowManager;
        private readonly MetroWindow _metroWindow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly Func<EmployeeDto, EditEmployeeViewModel> _initEditEmployee;
        private EmployeeDto _selectedEmployee = null!;

        public MainWindowViewModel(
            CsvFileReader csvFileReader,
            IWindowManager windowManager,
            MetroWindow metroWindow,
            IEmployeeRepository employeeRepository,
            Func<EmployeeDto, EditEmployeeViewModel> initEditEmployee)
        {
            _csvFileReader = csvFileReader;
            _windowManager = windowManager;
            _metroWindow = metroWindow;
            _employeeRepository = employeeRepository;
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
            }
        }

        public async Task Delete()
        {
            try
            {
                var selectedEmployee = SelectedEmployee;
                var result = await _metroWindow.ShowMessageAsync("Usuń",$"Czy napewno chcesz usunąć użytkownika: {selectedEmployee.Name} {selectedEmployee.Surename}?", MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    Application.Current.Dispatcher.Invoke(() => Employees.Remove(selectedEmployee));
                    await _employeeRepository.DeleteEmployee(selectedEmployee);
                }
            }
            catch (Exception exception)
            {
                await _metroWindow.ShowMessageAsync("Błąd", exception.Message);
            }
        }

        public async Task Edit()
        {
            try
            {
                var selectedEmployee = SelectedEmployee;
                var edit = _initEditEmployee.Invoke(selectedEmployee);
                var result = await _windowManager.ShowDialogAsync(edit);

                var employeeIndex = Employees.IndexOf(selectedEmployee);
                if (employeeIndex < 0) return;

                Employees.RemoveAt(employeeIndex);
                Employees.Insert(employeeIndex, selectedEmployee);
                await _employeeRepository.UpdateEmployee(Employees[employeeIndex]);
            }
            catch (Exception exception)
            {
                await _metroWindow.ShowMessageAsync("Błąd", exception.Message);
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
                    await LoadEmployees(ofd.FileName);
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

        private async Task LoadEmployees(string filePath)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var employees = _csvFileReader.ReadAllEmployee(filePath).ToArray();
                    await _employeeRepository.SetNewEmployeesCollection(employees);
                    Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        Employees.Clear();
                        foreach (var employee in employees)
                        {
                            Employees.Add(employee);
                        }
                    });
                }
                catch (Exception exception)
                {
                    await _metroWindow.ShowMessageAsync("Błąd", exception.Message);
                }
            });
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
