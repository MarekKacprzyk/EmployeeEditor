using Caliburn.Micro;
using EmployeeEditor.Domain.Dtos;

namespace EmployeeEditor.WpfApp.ViewModels
{
    public class TagViewModel : PropertyChangedBase
    {
        public TagViewModel(TagDto tag)
        {
            Dto = tag;
        }

        public string Name
        {
            get => Dto?.Name ?? string.Empty;
            set
            {
                if (value == Dto.Name) return;
                Dto.Name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Description
        {
            get => Dto?.Description ?? string.Empty;
            set
            {
                if (value == Dto.Description) return;
                Dto.Description = value;
                NotifyOfPropertyChange();
            }
        }

        public TagDto Dto { get; }
    }
}
