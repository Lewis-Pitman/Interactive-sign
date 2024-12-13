using System.Globalization;
using Interactive_sign.Resources.StringLocalisation;
using Microsoft.VisualBasic;
using Microsoft.Maui.Controls;
using System.Security.Cryptography.X509Certificates;
using Interactive_sign.ViewModels;
using Interactive_sign.Classes;
using Interactive_sign.Pages;
using System.Xml.Linq;

namespace Interactive_sign;


public partial class Home : ContentPage
{
    private string currentTab = "home";
    private HomeViewModel homeViewModel;
    private ItemDatabase itemDatabase;
    private EventDatabase eventDatabase;


    public Home(string tab)
	{
		InitializeComponent();
        homeViewModel = new HomeViewModel();
        itemDatabase = new ItemDatabase();
        eventDatabase = new EventDatabase();

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

        //Initialise tabs

        InitialiseEventGrid();
        InitialiseSearchGrid();

        var mapContent = new WebView
        {
            Source = "https://www.google.co.uk/maps/@51.3836602,-2.1981736,459505m",
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        mapView.Content = mapContent;
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
        var categoryDictionary = await itemDatabase.GetCategories(); //Used for getting image associated with category
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
            var categoryImageCentreItem = await itemDatabase.GetItem(categoryDictionary[category]);

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
            categoryItemGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
            categoryItemGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            //Button (Transparent button covering whole grid so that the user can click anywhere, even on the image and text)
            var categoryItemButton = new Button { BackgroundColor = Colors.Transparent, ZIndex = 5, AutomationId = category };
            categoryItemButton.Clicked += CategoryItemButtonClicked;
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
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Aspect = Aspect.AspectFill
            };

            var imageFrame = new Frame {
                BackgroundColor = Color.FromArgb(backgroundColours[colourIndex]),
                BorderColor = Color.FromArgb(backgroundColours[colourIndex]),
                ZIndex = 2,
                CornerRadius = 40,
                Padding = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Content = categoryItemImage
            };
            categoryItemGrid.Children.Add(imageFrame);
            categoryItemGrid.SetRow(imageFrame, 0);

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

    private async void CategoryItemButtonClicked(object sender, EventArgs e)
    {
        var categoryItem = sender as Button;
        await Navigation.PushAsync(new CategoryDetails(categoryItem.AutomationId), false);
    }

    //---------------------------------------------------------------------------------

    //Events --------------------------------------------------------------------------

    private async void EventButtonClicked(object sender, EventArgs e, int eventID)
    {
        await Navigation.PushAsync(new EventDetails(eventID), false);
    }

    private async void LoadEventItems()
    {
        var todaysEvents = await eventDatabase.GetTodaysEvents();

        int rowsToMake = todaysEvents.Count();
        int rowCount = 1;

        string[] backgroundColours = { "c96e6d", "6673ad", "c6d7ad", "ffdfa3" };
        string[] borderColours = { "b80c09", "4357ad", "a5cc6b", "fcab10" };
        int colourIndex = 0;

        for (int i = 0; i < rowsToMake; i++) { eventsTabGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); }

        foreach (EventItem item in todaysEvents)
        {
            var backgroundGrid = new Grid { ZIndex = 10, HeightRequest = 200 };

            var itemGrid = new Grid { Padding = 20 };
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //Image 
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) }); //Title
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //Info icon


            //Background
            var itemBorder = new BoxView
            {
                BackgroundColor = Color.FromArgb(borderColours[colourIndex]),
                CornerRadius = 40,
                ZIndex = 0
            };

            var itemBackground = new BoxView
            {
                BackgroundColor = Color.FromArgb(backgroundColours[colourIndex]),
                CornerRadius = 40,
                ZIndex = 1
            };


            backgroundGrid.Children.Add(itemBorder);
            itemGrid.Children.Add(itemBackground);
            itemGrid.SetColumnSpan(itemBackground, 3);

            //Button
            var itemButton = new Button
            {
                BackgroundColor = Colors.Transparent,
                AutomationId = item.ItemID.ToString(),
                ZIndex = 5
            };

            itemButton.Clicked += (sender, e) => EventButtonClicked(sender, e, item.EventID);
            backgroundGrid.Children.Add(itemButton);



            //Image (If null, image is taken from itemID)
            if (item.EventImage != null)
            {
                var itemImage = new Image
                {
                    Source = ImageSource.FromStream(() => new MemoryStream(item.EventImage)),
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    MaximumWidthRequest = 400,
                    Aspect = Aspect.AspectFill
                };

                var imageFrame = new Frame
                {
                    BorderColor = Color.FromArgb(backgroundColours[colourIndex]),
                    ZIndex = 2,
                    CornerRadius = 40,
                    Padding = 0,
                    VerticalOptions = LayoutOptions.Fill,
                    MaximumWidthRequest = 400,
                    Content = itemImage
                };

                itemGrid.Children.Add(imageFrame);
                itemGrid.SetColumn(imageFrame, 0);
            }
            else
            {
                var itemFromEvent = await itemDatabase.GetItem(item.ItemID);

                var itemImage = new Image
                {
                    Source = ImageSource.FromStream(() => new MemoryStream(itemFromEvent.ItemImage)),
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    Aspect = Aspect.AspectFill,
                    MaximumWidthRequest = 400
                };

                var imageFrame = new Frame
                {
                    BorderColor = Color.FromArgb(backgroundColours[colourIndex]),
                    ZIndex = 2,
                    CornerRadius = 40,
                    Padding = 0,
                    VerticalOptions = LayoutOptions.Fill,
                    MaximumWidthRequest = 400,
                    Content = itemImage
                };

                itemGrid.Children.Add(imageFrame);
                itemGrid.SetColumn(imageFrame, 0);
            }

            //Text
            var itemLabel = new Label
            {
                Text = item.EventTitle,
                TextColor = Colors.White,
                FontSize = Settings.Instance.Scale,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                ZIndex = 2
            };

            itemGrid.Children.Add(itemLabel);
            itemGrid.SetColumn(itemLabel, 1);

            //Icon
            var imageIcon = new Image
            {
                Source = "info.png",
                ZIndex = 2
            };

            itemGrid.Children.Add(imageIcon);
            itemGrid.SetColumn(imageIcon, 2);

            backgroundGrid.Children.Add(itemGrid);

            eventsTabGrid.Children.Add(backgroundGrid);
            eventsTabGrid.SetRow(backgroundGrid, rowCount);
            eventsTabGrid.SetColumn(backgroundGrid, (int)Math.Floor(item.StartTime / 30.0));
            eventsTabGrid.SetColumnSpan(backgroundGrid, (int)Math.Ceiling((item.EndTime - item.StartTime) / 30.0));

            colourIndex = (colourIndex >= 3) ? 0 : colourIndex + 1;
            rowCount++;
        }
    }

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
            var background = new BoxView { 
                BackgroundColor = Color.FromArgb(backgroundColours[backgroundColourCount]), 
                ZIndex = 1, 
                WidthRequest = 500, 
                HeightRequest = 1000,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            eventsTabGrid.Children.Add(background);
            eventsTabGrid.SetRow(background, 1);
            eventsTabGrid.SetRowSpan(background, 10);
            eventsTabGrid.SetColumn(background, i);

            //Increment background colour count to get the correct background colour sequence
            backgroundColourCount++;
            if(backgroundColourCount >= 4) { backgroundColourCount = 0; }

            //Add time to the top label
            if(i % 2 == 0)
            {
                string timeLabelText = timeCount.ToString() + ":00 ";
                timeLabelText += (timeCount >= 12) ? "PM" : "AM"; //If the time is 12 or above, add PM, else add AM to the string

                var timeLabel = new Label { TextColor = Color.FromArgb("FFFFFF"), FontSize = 60, Text=timeLabelText, ZIndex=5};
                eventsTabGrid.Children.Add(timeLabel);
                eventsTabGrid.SetRow(timeLabel, 0);
                eventsTabGrid.SetColumn(timeLabel, i);

                timeCount++;
            }

        }

        LoadEventItems();
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