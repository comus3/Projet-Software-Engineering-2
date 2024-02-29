namespace KitBox.Views;

public partial class FinishPage : ContentPage
{

	public FinishPage()
	{
		InitializeComponent();
		//BindingContext = this;
	}
    private void OnClicked(object sender, EventArgs e)
	{

		Navigation.PushAsync(new Customer_catalog());
	}

}