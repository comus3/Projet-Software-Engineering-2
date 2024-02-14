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

}