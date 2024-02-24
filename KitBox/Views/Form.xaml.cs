using Microsoft.UI.Xaml;
using DAL;

namespace KitBox.Views
{
    public partial class Form : ContentPage
    {
        int count = 1;
        string[] options = { "Option 1", "Option 2", "Option 3" };
        List<string> doorOptions = new List<string>
        {
            "Bleu Turquoise",
            "Mauve",
            "Rouge sang"
        };

        Connection connection; // Déclaration de la connexion à la base de données

        public Form()
        {
            InitializeComponent();
            connection = new Connection(); // Initialisation de la connexion
            //BindingContext = this;
        }

        private void OptionsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedOption = (string)OptionsPicker.SelectedItem;
            //ResultLabel.Text = $"Vous avez choisi l'option : {selectedOption}";
        }

        private void OptionsDoorPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedOption = (string)OptionsDoorPicker.SelectedItem;
            //ResultDoorLabel.Text = $"Vous avez choisi l'option : {selectedOption}";
        }

        private void OnFinishClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FinishPage());
        }

        private void OnCreateNewCasierClicked(object sender, EventArgs e)
        {
            count++;
            Label newLabel = new Label();
            newLabel.Text = "Locker number " + count.ToString();
            newLabel.FontAttributes = FontAttributes.Bold;
            newLabel.HorizontalOptions = LayoutOptions.Center;

            Entry entry = new Entry();
            entry.Placeholder = "Height " + count;

            Label Color = new Label();
            Color.Text = "Choose a color : ";

            Picker colorPicker = new Picker();
            foreach (string option in options)
            {
                colorPicker.Items.Add(option);
            }

            Label Door = new Label();
            Door.Text = "Include a door";
            Door.VerticalOptions = LayoutOptions.Center;
            CheckBox checkBox = new CheckBox();

            HorizontalStackLayout stackPanel = new HorizontalStackLayout();
            stackPanel.Children.Add(Door);
            stackPanel.Children.Add(checkBox);

            Label colorDoor = new Label();
            colorDoor.Text = "Choose a color for the door number " + count.ToString() + " : ";
            colorDoor.VerticalOptions = LayoutOptions.Center;
            Picker colorDoorPicker = new Picker();
            foreach (string option in doorOptions)
            {
                colorDoorPicker.Items.Add(option);
            }

            checkBox.CheckedChanged += (sender, e) =>
            {
                bool isChecked = ((CheckBox)sender).IsChecked;
                if (isChecked)
                {
                    stackPanel.Children.Add(colorDoor);
                    stackPanel.Children.Add(colorDoorPicker);
                }
                else
                {
                    stackPanel.Children.Remove(colorDoor);
                    stackPanel.Children.Remove(colorDoorPicker);
                }
            };

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

            labelContainer.Children.Add(newLabel);
            labelContainer.Children.Add(entry);
            labelContainer.Children.Add(Color);
            labelContainer.Children.Add(colorPicker);
            labelContainer.Children.Add(stackPanel);

            // Création d'un nouvel objet Casier et insertion dans la base de données
            Casier casier = new Casier(connection);
            casier.Insert();
        }

        private void OnDoorCheckboxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // 'Value' is a property of CheckedChangedEventArgs indicating the new checked state.
            doorColorLabel.IsVisible = isChecked;
            OptionsDoorPicker.IsVisible = isChecked;
        }
    }
}
