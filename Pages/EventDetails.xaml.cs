using Interactive_sign.Classes;

namespace Interactive_sign.Pages;

public partial class EventDetails : ContentPage
{
	private int eventID;
    private EventDatabase eventDatabase;
    private ItemDatabase itemDatabase;
	public EventDetails(int _eventID)
	{
		InitializeComponent();

		eventID = _eventID;
        eventDatabase = new EventDatabase();
        itemDatabase = new ItemDatabase();

        currentTimeLabel.Text = DateTime.Now.ToString("HH:mm tt");
        currentTimeLabel.FontSize = Settings.Instance.Scale;
        categoryLabel.FontSize = Settings.Instance.Scale;

        LoadEventInfo();
    }

    private async void LoadEventInfo()
    {
        var eventItem = await eventDatabase.GetEvent(eventID);
        var item = await itemDatabase.GetItem(eventItem.ItemID);

        //Description
        categoryLabel.Text = eventItem.EventTitle + " - " + item.ItemTitle;

        switch (Settings.Instance.Language)
        {
            case "fr":
                descriptionLabel.Text = eventItem.Fr_Description;
                break;
            case "de":
                descriptionLabel.Text = eventItem.De_Description;
                break;
            case "cy":
                descriptionLabel.Text = eventItem.Cy_Description;
                break;
            default:
                descriptionLabel.Text = eventItem.En_Description;
                break;
        }

        descriptionLabel.FontSize = Settings.Instance.Scale;

        //Image
        if (eventItem.EventImage != null)
        {

            var image = new Image
            {
                Source = ImageSource.FromStream(() => new MemoryStream(eventItem.EventImage)),
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            imageFrame.Content = image;
        }
        else
        {
            var image = new Image
            {
                Source = ImageSource.FromStream(() => new MemoryStream(item.ItemImage)),
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };
            imageFrame.Content = image;
        }

        //Opening time
        TimeSpan openTime = TimeSpan.FromMinutes(eventItem.StartTime);
        TimeSpan closeTime = TimeSpan.FromMinutes(eventItem.EndTime);
        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        DateTime openDateTime = new DateTime(1, 1, 1).Add(openTime);
        DateTime closeDateTime = new DateTime(1, 1, 1).Add(closeTime);

        string openTimeString = $"{openDateTime:HH:mm tt} - {closeDateTime:HH:mm tt}";

        openingHoursLabel.Text = openTimeString;
        openingHoursLabel.FontSize = Settings.Instance.Scale;

        if (currentTime >= openTime && currentTime < closeTime)
        {
            openStatusBackground.BackgroundColor = Color.FromArgb("52aa5e");
            openStatusLabel.Text = "NOW";
            openStatusLabel.FontSize = Settings.Instance.Scale;
        }
        else
        {
            openStatusBackground.BackgroundColor = Color.FromArgb("c73e1d");
            openStatusLabel.Text = "NOT NOW";
            openStatusLabel.FontSize = Settings.Instance.Scale;
        }

        //Address & map
        addressLabel.Text = item.ItemAddress;
        addressLabel.FontSize = Settings.Instance.Scale;

        var mapWebView = new WebView
        {
            Source = "https://www.google.co.uk/maps/@51.3836602,-2.1981736,459505m",
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };
        mapFrame.Content = mapWebView;
    }

    //Buttons -----------------------------------------------------

    private void MapClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Home("map"), false);
    }

    //-------------------------------------------------------------

    //Button effects ----------------------------------------------
    private async void CancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", false);
    }

    private void CancelPressed(object sender, EventArgs e) { cancelButton.BackgroundColor = Color.FromArgb("863838"); }

    private void CancelReleased(object sender, EventArgs e) { cancelButton.BackgroundColor = Color.FromArgb("c73e1d"); }

    //-------------------------------------------------------------
}