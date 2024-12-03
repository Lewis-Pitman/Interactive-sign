using System;
using System.Resources;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interactive_sign.Resources.StringLocalisation;

namespace Interactive_sign
{
    public static class LocalisationManager
    {
        private static readonly ResourceManager _resourceManager =
            new ResourceManager("Interactive_sign.Resources.StringLocalisation.UIText",
            typeof(uitext).Assembly);

        public static string GetString(string key)
        {
            CultureInfo.CurrentCulture = Settings.Instance.savedCulture; //Load from settings
            return _resourceManager.GetString(key, CultureInfo.CurrentCulture);
        }
    }
}
