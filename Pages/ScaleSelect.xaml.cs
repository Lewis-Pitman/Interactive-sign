using Interactive_sign.Classes;
using Interactive_sign.ViewModels;

namespace Interactive_sign;

public partial class ScaleSelect : ContentPage
{
    private bool cameFromSettings;
    private ScaleSelectViewModel scaleSelectViewModel;

	public ScaleSelect(bool _cameFromSettings)
	{
		InitializeComponent();
        scaleText.FontSize = 70;
        cameFromSettings = _cameFromSettings;

        scaleSelectViewModel = new ScaleSelectViewModel();
        BindingContext = scaleSelectViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Apply settings
        scaleText.FontSize = Settings.Instance.Scale;
        scaleSelectViewModel.ScaleText = LocalisationManager.GetString("ScaleAdjustableText");

    }

    private void ChangeScale(object sender, EventArgs e)
    {
        var button = sender as ImageButton;

        if (button.AutomationId == "increase" && scaleText.FontSize <= 100)
        {
            scaleText.FontSize += 10;
        }
        else if (button.AutomationId == "decrease" && scaleText.FontSize >= 30)
        {
            scaleText.FontSize -= 10;
        }
    }

    private async void Confirm(object sender, EventArgs e)
    {
        Settings.Instance.Scale = (int)scaleText.FontSize;

        if (cameFromSettings)
        {
            await Navigation.PushAsync(new Home("settings"), false);
        }
        else
        {
            await Navigation.PushAsync(new Accessibility(false));
        }

    }

    private void ButtonPressed(object sender, EventArgs e)
	{
		var button = sender as ImageButton;

		if(button.AutomationId == "confirm")
		{
			button.BackgroundColor = Color.FromArgb("31774e");
		}
		else
		{
            button.BackgroundColor = Color.FromArgb("000000");
        }
	}

    private void ButtonReleased(object sender, EventArgs e)
    {
        var button = sender as ImageButton;

        if (button.AutomationId == "confirm")
        {
            button.BackgroundColor = Color.FromArgb("52aa5e");
        }
        else
        {
            button.BackgroundColor = Color.FromArgb("262626");
        }
    }
}