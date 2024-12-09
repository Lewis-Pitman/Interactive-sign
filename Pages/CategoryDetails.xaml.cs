using Interactive_sign.Classes;
using Interactive_sign.Resources;

namespace Interactive_sign.Pages;

public partial class CategoryDetails : ContentPage
{
	private string category;
    private ItemDatabase database;

    public CategoryDetails(string _category)
	{
		InitializeComponent();
		category = _category;
		database = new ItemDatabase();
		
		InitialiseItems();

		headerMessage.FontSize = Settings.Instance.Scale;
	}

	private async void InitialiseItems()
	{
		var itemsInCategory = await database.GetItemsInCategory(category);

		int rowsToMake = itemsInCategory.Count();
		int rowCount = 0;

        string[] backgroundColours = { "c96e6d", "6673ad", "c6d7ad", "ffdfa3" };
        string[] borderColours = { "b80c09", "4357ad", "a5cc6b", "fcab10" };
		int colourIndex = 0;

        for (int i = 0; i < rowsToMake; i++) { itemList.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); }

		foreach(CentreItem item in itemsInCategory)
		{
			var backgroundGrid = new Grid();

			var itemGrid = new Grid { Padding = 20};
            itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); //Image 
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

			itemButton.Clicked += (sender, e) => ItemButtonClicked(sender, e, item.ItemID);
            backgroundGrid.Children.Add(itemButton);



            //Image
            var itemImage = new Image
			{
				Source = ImageSource.FromStream(() => new MemoryStream(item.ItemImage))
			};

            var imageFrame = new Frame
            {
                BorderColor = Color.FromArgb(backgroundColours[colourIndex]),
                ZIndex = 2,
                CornerRadius = 40,
                Padding = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Content = itemImage
            };

            itemGrid.Children.Add(imageFrame);
			itemGrid.SetColumn(imageFrame, 0);

			//Text
			var itemLabel = new Label
			{
				Text = item.ItemTitle,
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

            itemList.Children.Add(backgroundGrid);
			itemList.SetRow(backgroundGrid, rowCount);

            colourIndex = (colourIndex >= 3) ? 0 : colourIndex + 1;
            rowCount++;
		}
	}

	private async void ItemButtonClicked(object sender, EventArgs e, int ID)
	{
        await Navigation.PushAsync(new ItemDetails(ID));
    }

    //Button effects ----------------------------------------------
    private async void CancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", false);
    }

    private void CancelPressed(object sender, EventArgs e) { cancelButton.BackgroundColor = Color.FromArgb("863838"); }

    private void CancelReleased(object sender, EventArgs e) { cancelButton.BackgroundColor = Color.FromArgb("c73e1d"); }

    //-------------------------------------------------------------
}