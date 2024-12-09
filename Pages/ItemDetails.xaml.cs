using Interactive_sign.Classes;
using Interactive_sign.Resources;

namespace Interactive_sign.Pages;

public partial class ItemDetails : ContentPage
{
	private int ID;
    private ItemDatabase database;

	public ItemDetails(int _ID)
	{
		InitializeComponent();
        database = new ItemDatabase();
		ID = _ID;

        currentTimeLabel.Text = DateTime.Now.ToString("HH:mm tt");
        currentTimeLabel.FontSize = Settings.Instance.Scale;
        categoryLabel.FontSize = Settings.Instance.Scale;

        LoadItemInfo();
    }

    private async void LoadItemInfo()
    {
        CentreItem item = await database.GetItem(ID);
        categoryLabel.Text = item.ItemCategory;

        //Title
        titleLabel.Text = item.ItemTitle;
        titleLabel.FontSize = Settings.Instance.Scale + 30;

        //Image
        var image = new Image 
        { 
            Source = ImageSource.FromStream(() => new MemoryStream(item.ItemImage)),
            Aspect = Aspect.AspectFill,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };
        imageFrame.Content = image;

        //Opening time
        TimeSpan openTime = TimeSpan.FromMinutes(item.ItemOpenTime);
        TimeSpan closeTime = TimeSpan.FromMinutes(item.ItemCloseTime);
        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        DateTime openDateTime = new DateTime(1, 1, 1).Add(openTime);
        DateTime closeDateTime = new DateTime(1, 1, 1).Add(closeTime);

        string openTimeString = $"{openDateTime:HH:mm tt} - {closeDateTime:HH:mm tt}";

        openingHoursLabel.Text = openTimeString;
        openingHoursLabel.FontSize = Settings.Instance.Scale;

        if(currentTime >= openTime && currentTime < closeTime)
        {
            openStatusBackground.BackgroundColor = Color.FromArgb("52aa5e");
            openStatusLabel.Text = "OPEN";
            openStatusLabel.FontSize = Settings.Instance.Scale;
        }
        else
        {
            openStatusBackground.BackgroundColor = Color.FromArgb("c73e1d");
            openStatusLabel.Text = "CLOSED";
            openStatusLabel.FontSize = Settings.Instance.Scale;
        }

        //Address & map
        addressLabel.Text = item.ItemAddress;
        addressLabel.FontSize = Settings.Instance.Scale;
        mapMessageLabel.FontSize = Settings.Instance.Scale;

        var mapWebView = new WebView
        {
            Source = "https://www.google.co.uk/maps/@50.9451166,-2.6374569,4080m",
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