using Prism.Mvvm;
using System.Windows.Input;

namespace CloudClassroom.Models
{
    public class DeviceModel : BindableBase
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ICommand SelectCommand { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public DeviceModel(string id, string name, bool isSelected)
        {
            Id = id;
            Name = name;
            IsSelected = isSelected;
        }
    }
}
