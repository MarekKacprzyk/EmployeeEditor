using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.Domain.Interfaces;
using EmployeeEditor.WpfApp.Models.Validators;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public sealed class EditEmployeeViewModel : Screen, IDataErrorInfo
    {
        private readonly EmployeeDto _employee;
        private readonly EmployeeValidator _employeeValidator;
        private readonly TagValidator _tagValidator;
        private readonly IWindowManager _windowManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITagRepository _tagRepository;
        private readonly Func<TagDto, TagViewModel> _tagVmFactory;
        private readonly Func<string, MessageBoxViewModel> _messageBoxFactory;
        private TagViewModel _selectedTag;

        public EditEmployeeViewModel(EmployeeDto employee, 
            EmployeeValidator employeeValidator, 
            TagValidator tagValidator, 
            IWindowManager windowManager,
            IEmployeeRepository employeeRepository,
            ITagRepository tagRepository,
            Func<TagDto, TagViewModel> tagVmFactory,
            Func<EmployeeDto, TagEditorViewModel> tagEditorVmFactory,
            Func<string, MessageBoxViewModel> messageBoxFactory)
        {
            _employee = employee;
            _employeeValidator = employeeValidator;
            _tagValidator = tagValidator;
            _windowManager = windowManager;
            _employeeRepository = employeeRepository;
            _tagRepository = tagRepository;
            _tagVmFactory = tagVmFactory;
            _messageBoxFactory = messageBoxFactory;
            DisplayName = "Edit employee panel";
            
            Tags = new ObservableCollection<TagViewModel>(employee.Tags.Select(tagVmFactory.Invoke));
            TagsEditor = tagEditorVmFactory.Invoke(employee);
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

        public ObservableCollection<TagViewModel> Tags { get; }
        
        public TagEditorViewModel TagsEditor { get; set; }

        public TagViewModel SelectedTag
        {
            get => _selectedTag;
            set
            {
                Set(ref _selectedTag, value);
                NotifyOfPropertyChange(nameof(CanRemoveTag));
            }
        }

        public string Error { get; set; }

        public string this[string columnName]
        {
            get
            {
                var result = _employeeValidator.Validate(_employee);
                
                if (result.IsValid) return null;

                var selectedErro = result.Errors.FirstOrDefault(e => e.PropertyName == columnName);
                return selectedErro?.ErrorMessage;
            }
        }

        public async Task AddTag()
        {
            try
            {
                var newTag = new TagDto("nowy tag", "opis tagu");
                var response = await _tagRepository.AddTag(newTag, _employee);

                if (response is null) throw new DataException("Nie udało się dodać nowego tagu");
                
                Tags.Add(_tagVmFactory.Invoke(response));

            }
            catch (Exception exception)
            {
                await _windowManager.ShowDialogAsync(exception.Message);
            }
        }

        public bool CanRemoveTag => SelectedTag is not null;

        public async Task RemoveTag()
        {
            if (SelectedTag is null) return;
            
            Tags.Remove(SelectedTag);
            await _tagRepository.DeleteTag(SelectedTag.Dto, _employee);
            SelectedTag = null;
        }

        public async Task Exit()
        {
            await TryCloseAsync();
        }

        public override async Task<bool> CanCloseAsync(CancellationToken cancellationToken = new())
        {
            var result = await _employeeValidator.ValidateAsync(_employee, cancellationToken);
            var tagValidationResult= _employee.Tags.Select(p => _tagValidator.Validate(p)).Any(e => e.Errors.Any());

            var hasAnyError = result.Errors?.Any() ?? false;
            if (!hasAnyError && !tagValidationResult)
            {
                await _employeeRepository.UpdateEmployee(_employee);
                return true;
            }

            var messageBox = _messageBoxFactory.Invoke($"Nieprawidłowe dane:\n\n{result.Errors[0].ErrorMessage}");

            await _windowManager.ShowDialogAsync(messageBox);

            return false;
        }
    }
}
