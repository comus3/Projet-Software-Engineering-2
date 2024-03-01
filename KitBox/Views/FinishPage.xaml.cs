namespace KitBox.Views;
using DevTools;
using DAL;
public partial class FinishPage : ContentPage
{
	private static Connection connection;
	public FinishPage()
	{
		InitializeComponent();
		//BindingContext = this;
		Connection.TestConnection();

		connection = new Connection();
	}
    private void OnClicked(object sender, EventArgs e)
	{
		LogAllCabinets();
		Navigation.PushAsync(new Customer_catalog());
	}

	private static void LogAllCabinets()
	{
		Armoire armoire = new Armoire(connection);
		Dictionary<string, object> empty = new Dictionary<string, object>();
		Displayer.DisplayData(armoire.LoadAll(empty));


	}

}