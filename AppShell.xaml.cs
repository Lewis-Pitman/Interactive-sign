namespace Interactive_sign
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("LanguageConfirmation", typeof(LanguageConfirm));
        }
    }
}
