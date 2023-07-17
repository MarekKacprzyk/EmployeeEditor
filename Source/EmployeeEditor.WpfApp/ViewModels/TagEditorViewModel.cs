using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using EmployeeEditor.WpfApp.Models.Validators;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public class TagEditorViewModel : Screen, IDisposable, IDataErrorInfo
    {
        private readonly EmployeeDto _employee;
        private readonly TagValidator _tagValidator;
        private readonly Func<TagDto, TagViewModel> _tagVmFactory;
        private TagViewModel _selectedTag;

        public TagEditorViewModel(EmployeeDto employee, TagValidator tagValidator, Func<TagDto, TagViewModel> tagVmFactory)
        {
            _employee = employee;
            _tagValidator = tagValidator;
            _tagVmFactory = tagVmFactory;

            var tags = _employee.Tags.Select(_tagVmFactory.Invoke);
            Tags = new ObservableCollection<TagViewModel>(tags);
        }

        public ObservableCollection<TagViewModel> Tags { get; }

        public TagViewModel SelectedTag
        {
            get => _selectedTag;
            set
            {
                if (_selectedTag == value) return;
                _selectedTag = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(TagName));
                NotifyOfPropertyChange(nameof(TagDescription));
            }
        }

        public string TagName
        {
            get => _selectedTag?.Name ?? string.Empty;
            set
            {
                if (_selectedTag is null || value == _selectedTag.Name) return;
                _selectedTag.Name = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(Tags));
            }
        }

        public string TagDescription
        {
            get => _selectedTag?.Description ?? string.Empty;
            set
            {
                if (_selectedTag is null || value == _selectedTag.Description) return;
                _selectedTag.Description = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(Tags));
            }
        }
        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                if (SelectedTag is null) return null;
                var result = _tagValidator.Validate(SelectedTag.Dto);

                if (result.IsValid) return null;

                var selectedErro = result.Errors.FirstOrDefault(e => e.PropertyName == columnName);
                return selectedErro?.ErrorMessage;
            }
        }

        public async Task RemoveTag()
        {
            _employee.Tags.Remove(SelectedTag.Dto);
            Tags.Remove(SelectedTag);
            SelectedTag = null;
        }

        public async Task AddTag()
        {
            var newTag = new TagDto("newTag", "Description");
            _employee.Tags.Add(newTag);
            Tags.Add(_tagVmFactory.Invoke(newTag));
        }

        public void Dispose()
        {
        }
    }
}
