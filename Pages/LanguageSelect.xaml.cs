using System.Diagnostics;

namespace Interactive_sign
{
    public partial class LanguageSelect : ContentPage
    {
        private bool cameFromSettings;

        //Set cameFromSettings to false when no parameter is given (causes an exception otherwise)
        public LanguageSelect() : this(false) {}

        public LanguageSelect(bool _cameFromSettings)
        {
            InitializeComponent();
            cameFromSettings = _cameFromSettings;
        }

        private async void LanguageClicked(object sender, EventArgs e)
        {
            var language = sender as ImageButton;

            switch (language.AutomationId)
            {
                case "english":
                    await Navigation.PushAsync(new LanguageConfirm("en", cameFromSettings));
                    break;
                case "welsh":
                    await Navigation.PushAsync(new LanguageConfirm("cy", cameFromSettings));

                    break;
                case "french":
                    await Navigation.PushAsync(new LanguageConfirm("fr", cameFromSettings));

                    break;
                case "german":
                    await Navigation.PushAsync(new LanguageConfirm("de", cameFromSettings));
                    break;
            }
        }

        private void FlagPressed(object sender, EventArgs e)
        {
            var flag = sender as ImageButton;
            flag.Opacity = 0.5; //Change opacity to indicate which flag was pressed (accessibility)

        }

        private void FlagReleased(object sender, EventArgs e)
        {
            var flag = sender as ImageButton;
            flag.Opacity = 1; //Change opacity to indicate which flag was pressed (accessibility)

        }
    }

}
