using Microsoft.Maui.Controls.PlatformConfiguration;

namespace Interactive_sign
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

        }
    }
}
