using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactive_sign.ViewModels
{
    class AccessibilityViewModel : INotifyPropertyChanged
    {

        private string headerText;
        private string bionicReading;
        private string screenReader;
        private string guidanceMessage;

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

        public string HeaderText
        {
            get => headerText;
            set => SetProperty(ref headerText, value, nameof(HeaderText));
        }

        public string BionicReading
        {
            get => bionicReading;
            set => SetProperty(ref bionicReading, value, nameof(BionicReading));
        }

        public string ScreenReader
        {
            get => screenReader;
            set => SetProperty(ref screenReader, value, nameof(ScreenReader));
        }

        public string GuidanceMessage
        {
            get => guidanceMessage;
            set => SetProperty(ref guidanceMessage, value, nameof(GuidanceMessage));
        }
    }
}
