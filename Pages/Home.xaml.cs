using System.Globalization;
using Interactive_sign.Resources.StringLocalisation;
using Microsoft.VisualBasic;
using Microsoft.Maui.Controls;
using System.Security.Cryptography.X509Certificates;
using Interactive_sign.ViewModels;

namespace Interactive_sign;


public partial class Home : ContentPage
{
    private string currentTab = "home";
    private HomeViewModel homeViewModel;

    public Home(string tab)
	{
		InitializeComponent();
        homeViewModel = new HomeViewModel();

        BindingContext = homeViewModel;

        currentTimeLabel.Text = DateTime.Now.ToString("HH:mm tt");

        //Set up the page so it starts on the chosen tab
        currentTab = tab;

        var selectedTab = FindByName(tab + "Button") as ImageButton;
        var selectedTabContentView = FindByName(tab + "Tab") as ContentView;

        mapTab.IsVisible = false;
        homeTab.IsVisible = false;
        eventsTab.IsVisible = false;
        settingsTab.IsVisible = false;

        mapButton.BackgroundColor = Color.FromArgb("262626");
        homeButton.BackgroundColor = Color.FromArgb("262626");
        eventsButton.BackgroundColor = Color.FromArgb("262626");
        settingsButton.BackgroundColor = Color.FromArgb("262626");

        selectedTab.BackgroundColor = Color.FromArgb("4b4b4b");
        selectedTabContentView.IsVisible = true;

        //-----------------------------------------------

        InitialiseEventGrid();

        
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Apply font size settings ---

        //Header
        shoppingCentreName.FontSize = Settings.Instance.Scale + 20;
        currentTimeLabel.FontSize = Settings.Instance.Scale + 20;
        
        //Map
        mapButton1.FontSize = Settings.Instance.Scale;
        mapButton2.FontSize = Settings.Instance.Scale;
        mapButton3.FontSize = Settings.Instance.Scale;
        mapButton4.FontSize = Settings.Instance.Scale;

        //Settings
        languageLabel.FontSize = (Settings.Instance.Scale > 60) ? 60 : Settings.Instance.Scale; //Cap size of this label to fit in the box
        fontLabel.FontSize = Settings.Instance.Scale;
        accessibilityLabel.FontSize = Settings.Instance.Scale;
        helpLabel.FontSize = Settings.Instance.Scale;

        //Apply language settings ---

        homeViewModel.CentreNameText = LocalisationManager.GetString("ShoppingCentreName");

        //Settings
        homeViewModel.FontSizeSettingText = LocalisationManager.GetString("FontSizeSettingText");
        homeViewModel.AccessibilitySettingText = LocalisationManager.GetString("AccessibilitySettingText");
        homeViewModel.HelpSettingText = LocalisationManager.GetString("HelpSettingText");

        //Map
        homeViewModel.MapWhereAmI = LocalisationManager.GetString("MapWhereAmI");
        homeViewModel.MapMoveMap = LocalisationManager.GetString("MapMoveMap");
        homeViewModel.MapZoomOut = LocalisationManager.GetString("MapZoomOut");
        homeViewModel.MapZoomIn = LocalisationManager.GetString("MapZoomIn");

    }

    //Dock ----------------------------------------------------------------------------

    private void DockPressed(object sender, EventArgs e)
	{
        var button = sender as ImageButton;
        button.BackgroundColor = Color.FromArgb("000000");
	}

    private void DockReleased(object sender, EventArgs e)
    {
        var button = sender as ImageButton;

        if (button.AutomationId == currentTab)
        {
            button.BackgroundColor = Color.FromArgb("4b4b4b");
        }
        else
        {
            button.BackgroundColor = Color.FromArgb("262626");
        }
    }

    private void DockClicked(object sender, EventArgs e)
    {
        currentTimeLabel.Text = DateTime.Now.ToString("HH:mm tt"); //Time only updates when buttons are pressed

        var button = sender as ImageButton;
        currentTab = button.AutomationId;

        switch (currentTab)
        {
            case "events":
                eventsTab.IsVisible = true;
                eventsButton.BackgroundColor = Color.FromArgb("4b4b4b");

                homeTab.IsVisible = false;
                mapTab.IsVisible = false;
                settingsTab.IsVisible = false;
                homeButton.BackgroundColor = Color.FromArgb("262626");
                mapButton.BackgroundColor = Color.FromArgb("262626");
                settingsButton.BackgroundColor = Color.FromArgb("262626");
                break;
            case "map":
                mapTab.IsVisible = true;
                mapButton.BackgroundColor = Color.FromArgb("4b4b4b");

                homeTab.IsVisible = false;
                eventsTab.IsVisible = false;
                settingsTab.IsVisible = false;
                homeButton.BackgroundColor = Color.FromArgb("262626");
                eventsButton.BackgroundColor = Color.FromArgb("262626");
                settingsButton.BackgroundColor = Color.FromArgb("262626");
                break;
            case "settings":
                settingsTab.IsVisible = true;
                settingsButton.BackgroundColor = Color.FromArgb("4b4b4b");

                homeTab.IsVisible = false;
                mapTab.IsVisible = false;
                eventsTab.IsVisible = false;
                homeButton.BackgroundColor = Color.FromArgb("262626");
                eventsButton.BackgroundColor = Color.FromArgb("262626");
                mapButton.BackgroundColor = Color.FromArgb("262626");
                break;
            default:
                homeTab.IsVisible = true;
                homeButton.BackgroundColor = Color.FromArgb("4b4b4b");

                eventsTab.IsVisible = false;
                mapTab.IsVisible = false;
                settingsTab.IsVisible = false;
                mapButton.BackgroundColor = Color.FromArgb("262626");
                eventsButton.BackgroundColor = Color.FromArgb("262626");
                settingsButton.BackgroundColor = Color.FromArgb("262626");
                break;
        }
    }

    //---------------------------------------------------------------------------------

    //Events --------------------------------------------------------------------------
    
    private void InitialiseEventGrid()
    {
        string[] backgroundColours = { "2e2e2e", "414141", "c1c1c1", "ffffff" };
        int backgroundColourCount = 0;
        int timeCount = 0;

        for(int i = 0; i < 48; i++)
        {
            //Add column definitions
            eventsTabGrid.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Auto });
            
            //Add background
            var background = new BoxView { BackgroundColor = Color.FromArgb(backgroundColours[backgroundColourCount]), ZIndex = 1, WidthRequest = 300, HeightRequest = 800 };
            eventsTabGrid.Children.Add(background);
            eventsTabGrid.SetRow(background, 1);
            eventsTabGrid.SetColumn(background, i);

            //Increment background colour count to get the correct background colour sequence
            backgroundColourCount++;
            if(backgroundColourCount >= 4) { backgroundColourCount = 0; }

            //Add time to the top label
            if(i % 2 == 0)
            {
                string timeLabelText = timeCount.ToString() + ":00 ";
                timeLabelText += (timeCount >= 12) ? "PM" : "AM"; //If the time is 12 or above, add PM, else add AM to the string

                var timeLabel = new Label { TextColor = Color.FromArgb("FFFFFF"), FontSize = 60, Text=timeLabelText, ZIndex=2};
                eventsTabGrid.Children.Add(timeLabel);
                eventsTabGrid.SetRow(timeLabel, 0);
                eventsTabGrid.SetColumn(timeLabel, i);

                timeCount++;
            }

        }

        //Add top label (This adds a box in the middle of the screen, but works because the grid's background is the same colour. Alignment options don't work here)
        var topLabel = new BoxView { BackgroundColor = Color.FromArgb("4a4a4a"), ZIndex = 1, WidthRequest=10, HeightRequest=100};
        eventsTabGrid.Children.Add(topLabel);
        eventsTabGrid.SetRow(topLabel, 0);
        eventsTabGrid.SetColumnSpan(topLabel, 49);
    }

    //---------------------------------------------------------------------------------

    //Map -----------------------------------------------------------------------------

    private void MapButtonPressed(object sender, EventArgs e)
    {
        var buttonPressed = sender as Button;
        var buttonBackground = FindByName(buttonPressed.AutomationId) as BoxView;

        if(buttonBackground != null)
        {
            buttonBackground.BackgroundColor = Color.FromArgb("4357ad");
        }
    }

    private void MapButtonReleased(object sender, EventArgs e)
    {
        var buttonPressed = sender as Button;
        var buttonBackground = FindByName(buttonPressed.AutomationId) as BoxView;

        if (buttonBackground != null)
        {
            buttonBackground.BackgroundColor = Color.FromArgb("6673ad");
        }
    }

    private async void MapButtonClicked(object sender, EventArgs e)
    {
        //Google maps needs an API key to control the map with external buttons
    }

    //---------------------------------------------------------------------------------

    //Settings ------------------------------------------------------------------------


    private void SettingsButtonPressed(object sender, EventArgs e)
    {
        var button = sender as Button;

        switch (button.AutomationId)
        {
            case "languageButton":
                languageBackground.BackgroundColor = Color.FromArgb("b80c09");
                break;
            case "scaleButton":
                scaleBackground.BackgroundColor = Color.FromArgb("4357ad");
                break;
            case "accessibilityButton":
                accessibilityBackground.BackgroundColor = Color.FromArgb("a5cc6b");
                break;
            case "helpButton":
                helpBackground.BackgroundColor = Color.FromArgb("fcab10");
                break;
        }
    }
    private void SettingsButtonReleased(object sender, EventArgs e)
    {
        var button = sender as Button;

        switch (button.AutomationId)
        {
            case "languageButton":
                languageBackground.BackgroundColor = Color.FromArgb("c96e6d");
                break;
            case "scaleButton":
                scaleBackground.BackgroundColor = Color.FromArgb("6673ad");
                break;
            case "accessibilityButton":
                accessibilityBackground.BackgroundColor = Color.FromArgb("c6d7ad");
                break;
            case "helpButton":
                helpBackground.BackgroundColor = Color.FromArgb("ffdfa3");
                break;
        }
    }
    private async void SettingsButtonClicked(object sender, EventArgs e)
    {
        var itemSelected = sender as Button;

        switch (itemSelected.AutomationId)
        {
            case "languageButton":
                await Navigation.PushAsync(new LanguageSelect(true), false);
                break;
            case "scaleButton":
                await Navigation.PushAsync(new ScaleSelect(true), false);
                break;
            case "accessibilityButton":
                await Navigation.PushAsync(new Accessibility(true), false);
                break;
            case "helpButton":
                await Navigation.PushAsync(new HelpScreen(), false);
                break;
        }
    }

    //---------------------------------------------------------------------------------

}