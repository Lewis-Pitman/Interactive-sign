using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactive_sign
{
    public class Settings
    {
        //This class is a singleton, meaning there is only one object

        private static Settings _instance; //Stores the single instance of the object

        public string Language { get; set; }
        public int Scale { get; set; }
        public bool Bionic { get; set; }
        public bool ScreenReader { get; set; }

        private Settings() { //Default values
            Language = "english";
            Scale = 80;
            Bionic = false;
            ScreenReader = true; //On by default in case user is blind
        }

        public static Settings Instance => _instance ??= new Settings(); //Create a new object of "Settings". If _instance is null, it is assigned to new Settings()
    }
}
