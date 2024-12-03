using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Interactive_sign.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        //Header
        private string centreNameText;

        //Settings
        private string fontSizeSettingText;
        private string accessibilitySettingText;
        private string helpSettingText;

        public string CentreNameText
        {
            get => centreNameText;
            set
            {
                if (centreNameText != value)
                {
                    centreNameText = value;
                    OnPropertyChanged(nameof(CentreNameText)); // Notify binding system
                }
            }
        }

        public string FontSizeSettingText
        {
            get => fontSizeSettingText;
            set
            {
                if (fontSizeSettingText != value)
                {
                    fontSizeSettingText = value;
                    OnPropertyChanged(nameof(FontSizeSettingText)); // Notify binding system
                }
            }
        }

        public string AccessibilitySettingText
        {
            get => accessibilitySettingText;
            set
            {
                if (accessibilitySettingText != value)
                {
                    accessibilitySettingText = value;
                    OnPropertyChanged(nameof(AccessibilitySettingText)); // Notify binding system
                }
            }
        }

        public string HelpSettingText
        {
            get => helpSettingText;
            set
            {
                if (helpSettingText != value)
                {
                    centreNameText = value;
                    OnPropertyChanged(nameof(HelpSettingText)); // Notify binding system
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}