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

        //Map
        private string mapWhereAmI;
        private string mapMoveMap;
        private string mapZoomOut;
        private string mapZoomIn;

        protected bool SetProperty<T>(ref T storage, T value, string propertyName)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        //Header
        public string CentreNameText
        {
            get => centreNameText;
            set => SetProperty(ref centreNameText, value, nameof(CentreNameText));
        }

        //Settings
        public string FontSizeSettingText
        {
            get => fontSizeSettingText;
            set => SetProperty(ref fontSizeSettingText, value, nameof(FontSizeSettingText));
        }

        public string AccessibilitySettingText
        {
            get => accessibilitySettingText;
            set => SetProperty(ref accessibilitySettingText, value, nameof(AccessibilitySettingText));
        }

        public string HelpSettingText
        {
            get => helpSettingText;
            set => SetProperty(ref helpSettingText, value, nameof(HelpSettingText));
        }

        //Map
        public string MapWhereAmI
        {
            get => mapWhereAmI;
            set => SetProperty(ref mapWhereAmI, value, nameof(MapWhereAmI));
        }

        public string MapMoveMap
        {
            get => mapMoveMap;
            set => SetProperty(ref mapMoveMap, value, nameof(MapMoveMap));
        }

        public string MapZoomOut
        {
            get => mapZoomOut;
            set => SetProperty(ref mapZoomOut, value, nameof(MapZoomOut));
        }
        public string MapZoomIn
        {
            get => mapZoomIn;
            set => SetProperty(ref mapZoomIn, value, nameof(MapZoomIn));
        }

    }
}