using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using EmployeeEditor.WpfApp.Models.Csv;
using Microsoft.Win32;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public class MainWindowViewModel : Screen
    {
        private readonly CsvFileReader _csvFileReader;

        public MainWindowViewModel(CsvFileReader csvFileReader)
        {
            _csvFileReader = csvFileReader;
        }

        public async Task Load()
        {
            var ofd = new OpenFileDialog();



            if (ofd.ShowDialog())
            {
                
            }
        }
    }
}
