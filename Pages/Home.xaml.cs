using System.Globalization;
using Interactive_sign.Resources.StringLocalisation;
using Microsoft.VisualBasic;
using Microsoft.Maui.Controls;
using System.Security.Cryptography.X509Certificates;
using Interactive_sign.ViewModels;
using Interactive_sign.Classes;

namespace Interactive_sign;


public partial class Home : ContentPage
{
    private string currentTab = "home";
    private HomeViewModel homeViewModel;
    private ItemDatabase database;


    public Home(string tab)
	{
		InitializeComponent();
        database = new ItemDatabase();
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
        InitialiseSearchGrid();
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

    //Search -----------------------------------------------------------------------------

    private async void InitialiseSearchGrid()
    {
        var categoryDictionary = await database.GetCategories(); //Used for getting image associated with category
        var categoryList = categoryDictionary.Keys.ToList<string>(); //List of categories
        int rowsToMake = (int)Math.Ceiling(categoryList.Count() / 2.0);

        string[] backgroundColours = { "c96e6d", "6673ad", "c6d7ad", "ffdfa3" };
        string[] borderColours = { "b80c09", "4357ad", "a5cc6b", "fcab10" };

        int colourIndex = 0;
        int rowCount = 0;
        int columnCount = 0;

        //Create the rows ready for categories to be added to them
        for(int i = 0; i < rowsToMake; i++) { categoryGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); }

        foreach (string category in categoryList)
        {
            var categoryImageCentreItem = await database.GetItem(categoryDictionary[category]);

            //Create the category item ------------------------------------------------------------ Height = new GridLength(3, GridUnitType.Star)

            var categoryItem = new Grid();

            //Add background
            
            var itemBackground = new BoxView
            {
                BackgroundColor = Color.FromArgb(borderColours[colourIndex]),
                CornerRadius = 30,
                ZIndex = 0
            };
            categoryGrid.Children.Add(itemBackground);
            categoryGrid.SetRow(itemBackground, rowCount);
            categoryGrid.SetColumn(itemBackground, columnCount);
            

            //Define individual category's grid
            var categoryItemGrid = new Grid { RowSpacing = 10, Padding = 20 };
            categoryItemGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            categoryItemGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            //Button (Transparent button covering whole grid so that the user can click anywhere, even on the image and text)
            var categoryItemButton = new Button { BackgroundColor = Colors.Transparent, ZIndex = 5 };
            categoryItemGrid.Children.Add(categoryItemButton);
            categoryItemGrid.SetRowSpan(categoryItemButton, 2);

            //Text 
            var categoryItemLabel = new Label
            {
                Text = category,
                FontSize = Settings.Instance.Scale,
                TextColor = Color.FromArgb("FFFFFF"),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                ZIndex = 3
            };

            categoryItemGrid.Children.Add(categoryItemLabel);
            categoryItemGrid.SetRow(categoryItemLabel, 1);

            var labelBackground = new Frame { 
                BackgroundColor = Color.FromArgb(backgroundColours[colourIndex]),
                BorderColor = Color.FromArgb(backgroundColours[colourIndex]),
                ZIndex = 2, 
                CornerRadius = 60, 
            };
            categoryItemGrid.Children.Add(labelBackground);
            categoryItemGrid.SetRow(labelBackground, 1);

            //Image
            var categoryItemImage = new Image
            {
                Source = ImageSource.FromStream(() => new MemoryStream(categoryImageCentreItem.ItemImage)),
                ZIndex = 3,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            categoryItemGrid.Children.Add(categoryItemImage);
            categoryItemGrid.SetRow(categoryItemImage, 0);

            var imageBackground = new Frame { 
                BackgroundColor = Color.FromArgb(backgroundColours[colourIndex]), 
                BorderColor = Color.FromArgb(backgroundColours[colourIndex]),
                ZIndex = 2, 
                CornerRadius = 60,
            };
            categoryItemGrid.Children.Add(imageBackground);
            categoryItemGrid.SetRow(imageBackground, 0);

            //------------------------------------------------------------------------------------

            //Add the created category to the grid of all categories
            categoryItem.Children.Add(categoryItemGrid);
            categoryGrid.Children.Add(categoryItem);
            categoryGrid.SetRow(categoryItem, rowCount);
            categoryGrid.SetColumn(categoryItem, columnCount);

            

            //Increment counts
            colourIndex = (colourIndex >= 3) ? 0 : colourIndex + 1;
            rowCount = (columnCount == 1) ? rowCount + 1 : rowCount;
            columnCount = (columnCount == 0) ? 1 : 0;
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