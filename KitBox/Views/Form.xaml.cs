using DAL;

namespace KitBox.Views
{
    public partial class Form : ContentPage
    {
        private Object armoirePk;
        private Connection con;
        private List<CasierData> casiersData = new List<CasierData>();

        int count = 0; // Compteur pour les casiers

        string[] options = { "marron", "white" };
        List<string> doorOptions = new List<string> { "marron", "white", "glass" };

        public Form(Object armoirePk)
        {
            this.armoirePk = armoirePk;
            InitializeComponent();
            BindingContext = this;
            Connection.TestConnection();
            con = new Connection();
        }

        private void OnCreateNewLockerClicked(object sender, EventArgs e)
        {
            count++;
            Label newLabel = new Label();
            newLabel.Text = "Locker number " + count.ToString();
            newLabel.FontAttributes = FontAttributes.Bold;
            newLabel.HorizontalOptions = LayoutOptions.Center;

            Entry entry = new Entry();
            entry.Placeholder = "Height " + count;

            Label colorLabel = new Label();
            colorLabel.Text = "Choose a color : ";

            Picker colorPicker = new Picker();
            foreach (string option in options)
            {
                colorPicker.Items.Add(option);
            }

            Label doorLabel = new Label();
            doorLabel.Text = "Include a door";
            doorLabel.VerticalOptions = LayoutOptions.Center;
            CheckBox checkBox = new CheckBox();

            HorizontalStackLayout stackPanel = new HorizontalStackLayout();
            stackPanel.Children.Add(doorLabel);
            stackPanel.Children.Add(checkBox);

            Label colorDoorLabel = new Label();
            colorDoorLabel.Text = "Choose a color for the door number " + count.ToString() + " : ";
            colorDoorLabel.VerticalOptions = LayoutOptions.Center;
            Picker colorDoorPicker = new Picker();
            foreach (string option in doorOptions)
            {
                colorDoorPicker.Items.Add(option);
            }

            checkBox.CheckedChanged += (sender, args) =>
            {
                bool isChecked = checkBox.IsChecked;
                if (isChecked)
                {
                    stackPanel.Children.Add(colorDoorLabel);
                    stackPanel.Children.Add(colorDoorPicker);
                }
                else
                {
                    stackPanel.Children.Remove(colorDoorLabel);
                    stackPanel.Children.Remove(colorDoorPicker);
                }
            };

            labelContainer.Children.Add(newLabel);
            labelContainer.Children.Add(entry);
            labelContainer.Children.Add(colorLabel);
            labelContainer.Children.Add(colorPicker);
            labelContainer.Children.Add(stackPanel);

            // Ajouter les données du casier actuel à la liste casiersData
            casiersData.Add(new CasierData
            {
                Color = colorPicker,
                DoorColor = colorDoorPicker,
                Height = entry,
                CheckBox = checkBox
            });
            Console.WriteLine(casiersData.ToString());
        }

        private async void OnFinishClicked(object sender, EventArgs e)
        {
            // Vérifier si les données de chaque casier sont valides et les ajouter à la base de données
            foreach (var casierData in casiersData)
            {
                if (casierData.Color.SelectedItem == null || casierData.Height.Text == null)
                {
                    await DisplayAlert("Error", "Make sure to choose a color and enter height", "OK");
                    return;
                }

                Casier casier = new Casier(con);
                Dictionary<string, object> infoCasier = new Dictionary<string, object>();

                infoCasier["couleur"] = casierData.Color.SelectedItem.ToString();
                infoCasier["h"] = casierData.Height.Text;
                infoCasier["porte"] = casierData.CheckBox.IsChecked;

                infoCasier["armoire"] = this.armoirePk;
                casier.Update(infoCasier);
                casier.Insert();
            }

            await Navigation.PushAsync(new FinishPage());
        }

        private class CasierData
        {
            public Picker Color { get; set; }
            public Picker DoorColor { get; set; }
            public Entry Height { get; set; }
            public CheckBox CheckBox { get; set; }
        }
    }
}





// checkBox.CheckedChanged += (sender, e) => {
		// 	bool isChecked = ((CheckBox)sender).IsChecked;
		// 	if (isChecked)
		// 	{
		// 		StackLayout doorStack = new StackLayout();
		// 		doorStack.Children.Add(colorDoor);
		// 		doorStack.Children.Add(colorDoorPicker);
		// 		labelContainer.Children.Insert(labelContainer.Children.IndexOf(stackPanel) + 1, doorStack);
		// 	}
		// 	else
		// 	{
		// 		foreach (View item in labelContainer.Children)
		// 		{
		// 			if (item is StackLayout && ((StackLayout)item).Children.Contains(colorDoor) && ((StackLayout)item).Children.Contains(colorDoorPicker))
		// 			{
		// 				labelContainer.Children.Remove(item);
		// 				break;
		// 			}
		// 		}
		// 	}
		// };
