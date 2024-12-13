using Interactive_sign.Classes;
using Interactive_sign.ViewModels;

namespace Interactive_sign;

public partial class HelpScreen : ContentPage
{
    private HelpViewModel helpViewModel;
    private int screen = 1;
    public HelpScreen()
    {
        InitializeComponent();
        helpViewModel = new HelpViewModel();
        BindingContext = helpViewModel;
        UpdateScreen();
    }

    private void UpdateScreen()
    {
        switch (screen)
        {
            case 2:
                var image2 = new Image
                {
                    Source = "help2.png",
                    Aspect = Aspect.AspectFill
                };
                imageFrame.Content = image2;
                helpViewModel.Text = LocalisationManager.GetString("Help2");
                break;
            case 3:
                var image3 = new Image
                {
                    Source = "help3.png",
                    Aspect = Aspect.AspectFill
                };
                imageFrame.Content = image3;
                helpViewModel.Text = LocalisationManager.GetString("Help3");
                break;
            default:
                var image1 = new Image
                {
                    Source = "help1.png",
                    Aspect = Aspect.AspectFill
                };
                imageFrame.Content = image1;
                helpViewModel.Text = LocalisationManager.GetString("Help1");
                break;
        }
    }

    // Left/right --------------------------------------------------------------------------------------------------------

    private void LeftClicked(object sender, EventArgs e)
    {
        screen = screen <= 1 ? 1 : screen - 1;
        UpdateScreen();
    }

    private void RightClicked(object sender, EventArgs e)
    {
        screen = screen >= 3 ? 3 : screen + 1;
        UpdateScreen();
    }

    //--------------------------------------------------------------------------------------------------------------------

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