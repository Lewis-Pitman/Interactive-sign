namespace Interactive_sign;

public partial class Accessibility : ContentPage
{
    private bool cameFromSettings;

    public Accessibility(bool _cameFromSettings)
	{
		InitializeComponent();
        cameFromSettings = _cameFromSettings;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Settings.Instance.Bionic)
        {
            BionicButton.BackgroundColor = Color.FromArgb("52aa5e");
            BionicButton.Text = "ON";
        }
        else
        {
            BionicButton.BackgroundColor = Color.FromArgb("c73e1d");
            BionicButton.Text = "OFF";
        }

        if (Settings.Instance.ScreenReader)
        {
            ScreenReaderButton.BackgroundColor = Color.FromArgb("52aa5e");
            ScreenReaderButton.Text = "ON";
        }
        else
        {
            ScreenReaderButton.BackgroundColor = Color.FromArgb("c73e1d");
            ScreenReaderButton.Text = "OFF";
        }
    }

    private void ButtonPressed(object sender, EventArgs e)
	{
		var pressedButton = sender as Button;

		if(pressedButton.Text == "ON")
		{
			pressedButton.BackgroundColor = Color.FromArgb("31774e");
		}
		else
		{
            pressedButton.BackgroundColor = Color.FromArgb("863838");
        }
    }

    private void ButtonReleased(object sender, EventArgs e)
    {
        var pressedButton = sender as Button;

        if (pressedButton.Text == "ON")
        {
            pressedButton.BackgroundColor = Color.FromArgb("52aa5e");
        }
        else
        {
            pressedButton.BackgroundColor = Color.FromArgb("c73e1d");
        }
    }

    private void ToggleButton(object sender, EventArgs e)
    {
        var pressedButton = sender as Button;

        if (pressedButton.Text == "ON")
        {
            pressedButton.BackgroundColor = Color.FromArgb("c73e1d");
            pressedButton.Text = "OFF";

            if(pressedButton.AutomationId == "BionicButton")
            {
                Settings.Instance.Bionic = false;
            }
            else
            {
                Settings.Instance.ScreenReader = false;
            }
        }
        else
        {
            pressedButton.BackgroundColor = Color.FromArgb("52aa5e");
            pressedButton.Text = "ON";

            if (pressedButton.AutomationId == "BionicButton")
            {
                Settings.Instance.Bionic = true;
            }
            else
            {
                Settings.Instance.ScreenReader = true;
            }
        }
    }

    private void ConfirmPressed(object sender, EventArgs e)
    {
        confirmButton.BackgroundColor = Color.FromArgb("31774e");
    }

    private void ConfirmReleased(object sender, EventArgs e)
    {
        confirmButton.BackgroundColor = Color.FromArgb("52aa5e");
    }

    private async void Confirm(object sender, EventArgs e)
    {
        if (cameFromSettings)
        {
            await Navigation.PushAsync(new Home("settings"), false);
        }
        else
        {
            await Navigation.PushAsync(new Home("home"));
        }
    }
}