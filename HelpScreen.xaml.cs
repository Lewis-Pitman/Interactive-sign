namespace Interactive_sign;

public partial class HelpScreen : ContentPage
{
    public HelpScreen()
    {
        InitializeComponent();
    }

    // Cancel button -----------------------------------------------------------------------------------------------------
    private void CancelPressed(object sender, EventArgs e) { cancelButton.BackgroundColor = Color.FromArgb("863838"); }
    private void CancelReleased(object sender, EventArgs e) { cancelButton.BackgroundColor = Color.FromArgb("c73e1d"); }
    private async void CancelClicked(object sender, EventArgs e) { await Shell.Current.GoToAsync("..", false); }
    //--------------------------------------------------------------------------------------------------------------------

    //Other buttons ----------------------------------------------------------------------------------------------------
    private void PressedButton(object sender, EventArgs e) {
        var buttonPressed = sender as Button;
        var buttonBackground = FindByName(buttonPressed.AutomationId) as BoxView;

        buttonBackground.Opacity = 0.65;
    }

    private void ReleasedButton(object sender, EventArgs e)
    {
        var buttonPressed = sender as Button;
        var buttonBackground = FindByName(buttonPressed.AutomationId) as BoxView;

        buttonBackground.Opacity = 1;
    }

    private void ClickedButton(object sender, EventArgs e)
    {
        var buttonPressed = sender as Button;
        var buttonBackground = FindByName(buttonPressed.AutomationId) as BoxView;

        buttonBackground.Opacity = 1;
    }
    //--------------------------------------------------------------------------------------------------------------------


}