namespace KitBox.Views;

public partial class Customer_catalog : ContentPage
{

	public Customer_catalog()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private void OnCreateCasierClicked(object sender, EventArgs e)
	{
		double length = Convert.ToDouble(lengthEntry.Text);
		double width = Convert.ToDouble(widthEntry.Text);

		DisplayAlert("Casier Created", $"Length: {length}, Width: {width}", "OK");
		Navigation.PushAsync(new Form());
	}
}