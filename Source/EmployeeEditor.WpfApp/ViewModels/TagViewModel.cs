using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;
using EmployeeEditor.WpfApp.Models.Validators;
using System.ComponentModel;
using System.Linq;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public class TagViewModel : Screen, IDataErrorInfo
    {
        private readonly TagValidator _validator;

        public TagViewModel(TagDto tag, TagValidator validator)
        {
            Dto = tag;
            _validator = validator;
        }

        public string Name
        {
            get => Dto.Name;
            set
            {
                if (value == Dto.Name) return;
                Dto.Name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Description
        {
            get => Dto.Name;
            set
            {
                if (value == Dto.Description) return;
                Dto.Description = value;
                NotifyOfPropertyChange();
            }
        }

        public string Error { get; }
        public TagDto Dto { get; set; }

        public string this[string columnName]
        {
            get
            {
                var result = _validator.Validate(Dto);

                if (result.IsValid) return null;

                var selectedErro = result.Errors.FirstOrDefault(e => e.PropertyName == columnName);
                return selectedErro?.ErrorMessage;
            }
        }
    }
}
