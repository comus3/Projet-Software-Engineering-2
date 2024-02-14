namespace KitBox.Views;

public partial class Form : ContentPage
{

	public Form()
	{
		InitializeComponent();
		//BindingContext = this;
	}

	private void OptionsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedOption = (string)OptionsPicker.SelectedItem;
            ResultLabel.Text = $"Vous avez choisi l'option : {selectedOption}";
        }
	
	private void OptionsDoorPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedOption = (string)OptionsDoorPicker.SelectedItem;
            ResultDoorLabel.Text = $"Vous avez choisi l'option : {selectedOption}";
        }

	private void OnFinishClicked(object sender, EventArgs e)
	{

		Navigation.PushAsync(new FinishPage());
	}

	private void OnCreateNewCasierClicked(object sender, EventArgs e)
	{
		
	}
	

}