using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDb.Classes
{
    public class Publisher : IDataErrorInfo, INotifyPropertyChanged
    {
        private readonly Dictionary<string, Func<string?>> _validationRules;

        public Publisher()
        {
            _validationRules = new Dictionary<string, Func<string?>>
            {
                { nameof(Name), () => string.IsNullOrWhiteSpace(Name) ? "🔴 Nesmí být prázdné" : null }
            };
        }

        public int? Id { get; set; }

        private string? _name;
        public string? Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(CanSave));
                }
            }
        }

        public string? Error => string.Join("\n", _validationRules.Values.Select(rule => rule()).Where(error => error != null));

        public string? this[string columnName] => _validationRules.ContainsKey(columnName) ? _validationRules[columnName]() : null;

        public bool CanSave => string.IsNullOrEmpty(Error);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
