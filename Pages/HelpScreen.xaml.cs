namespace Interactive_sign;

public partial class HelpScreen : ContentPage
{
    private int screen = 1;
    private string text1 = "On the search tab, you can scroll through categories by swiping your finger up and down on the screen. To see items inside a category, tap on the category.";
    private string text2 = "When you have tapped on a category, you can swipe your finger up and down to scroll through the available items, and tap on one that you'd like more information on.";
    private string text3 = "On the events tab, swipe your finger left and right on the timeline to view events. When you see one that interests you, tap on it for more information.";
    public HelpScreen()
    {
        InitializeComponent();
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
                textLabel.Text = text2;
                break;
            case 3:
                var image3 = new Image
                {
                    Source = "help3.png",
                    Aspect = Aspect.AspectFill
                };
                imageFrame.Content = image3;
                textLabel.Text = text3;
                break;
            default:
                var image1 = new Image
                {
                    Source = "help1.png",
                    Aspect = Aspect.AspectFill
                };
                imageFrame.Content = image1;
                textLabel.Text = text1;
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