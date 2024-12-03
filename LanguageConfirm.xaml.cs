namespace Interactive_sign;

public partial class LanguageConfirm : ContentPage
{

	private string language = "english";
	private bool cameFromSettings;

	public LanguageConfirm(string selectedLanguage, bool _cameFromSettings)
	{
		InitializeComponent();

		switch (selectedLanguage)
		{
			case "english":
				flagImage.Source = "flag_uk.png";
				flagName.Text = "English";
				break;
			case "welsh":
                flagImage.Source = "flag_wales.png";
                flagName.Text = "Cymraeg";
                break;
			case "french":
                flagImage.Source = "flag_france.png";
                flagName.Text = "Français";
                break;
			case "german":
                flagImage.Source = "flag_germany.png";
                flagName.Text = "Deutsch";
                break;
		}

		language = selectedLanguage;
		cameFromSettings = _cameFromSettings;
	}

	private async void ConfirmClicked(object sender, EventArgs e)
    {
		Settings.Instance.Language = language;

		if (cameFromSettings)
		{
            await Navigation.PushAsync(new Home("settings"), false);
        }
		else
		{
			await Navigation.PushAsync(new ScaleSelect(false));
		}

    }

	private async void CancelClicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
	}

	//Change colour when buttons pressed (For accessibility) -----------------------------------------------------------------

	private void ConfirmPressed(object sender, EventArgs e) { confirmButton.BackgroundColor = Color.FromArgb("31774e"); }
    private void CancelPressed(object sender, EventArgs e) { cancelButton.BackgroundColor = Color.FromArgb("863838"); }
	private void ConfirmReleased(object sender, EventArgs e) { confirmButton.BackgroundColor = Color.FromArgb("52aa5e"); }
	private void CancelReleased(object sender, EventArgs e) { cancelButton.BackgroundColor = Color.FromArgb("c73e1d"); }

    //------------------------------------------------------------------------------------------------------------------------



}