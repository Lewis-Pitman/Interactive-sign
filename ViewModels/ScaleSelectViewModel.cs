using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactive_sign.ViewModels
{
    class ScaleSelectViewModel : INotifyPropertyChanged
    {
        private string scaleText;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, string propertyName)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string ScaleText
        {
            get => scaleText;
            set => SetProperty(ref scaleText, value, nameof(ScaleText));
        }
    }
}
