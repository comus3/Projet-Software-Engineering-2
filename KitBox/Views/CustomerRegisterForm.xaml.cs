using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;
using KitBox.AppServices;
using AppServices;

namespace KitBox.Views;
// A implémenter : Visibilité des données de la commande (Date de la commande; Prix de la commande)
public partial class CustomerRegisterForm : ContentPage
{
	private Connection con;
	private Commande commande;
	object date;
	object price;
	private CustomerData customerData = new CustomerData(){
		Nom = new Entry(),
		Prenom = new Entry(),
		Telephone = new Entry(),
		Email = new Entry()
	};
	public CustomerRegisterForm()
	{
		BindingContext = this;
		InitializeComponent();
		string commandePk = FetchingServices.CurrentCommand;
		if (commandePk == "")
		{
			DisplayAlert("Error", "No order selected", "OK");
			commandePk = "1";
			return;
		}
		Connection.TestConnection();
		con = new Connection();
		this.commande = new Commande(con);
		DataTable res = this.commande.Load(commandePk);
		Displayer.DisplayData(res);
		this.date = res.Rows[0].ItemArray[1];
		this.price = res.Rows[0].ItemArray[5];
		
		BuildUI();


	}
	class CustomerData
	{
		public Entry Nom {get; set;}
		public Entry Prenom {get; set;}
		public Entry Telephone {get; set;}
		public Entry Email {get; set;}
	}
	private void BuildUI()
	{
		StackLayout stackLayout = new StackLayout();
		stackLayout.Children.Add(new Label
		{
			Text = "Last Name",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold
		});
		stackLayout.Children.Add(customerData.Nom);
		stackLayout.Children.Add(new Label
		{
			Text = "First Name",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold
		});
		stackLayout.Children.Add(customerData.Prenom);
		stackLayout.Children.Add(new Label
		{
			Text = "Phone Number",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold
		});
		stackLayout.Children.Add(customerData.Telephone);
		stackLayout.Children.Add(new Label
		{
			Text = "Email Address",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold
		});
		stackLayout.Children.Add(customerData.Email);
        Button submit = new Button
        {
            Text = "Submit",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            WidthRequest = 500,
            BackgroundColor = Color.FromArgb("#007ACC") // Utilisation de la couleur hexadécimale
        };
        submit.Clicked += OnSubmitClicked;
		stackLayout.Children.Add(submit);
		Content = stackLayout;
        if (StockServices.CheckAvailability(FetchingServices.CurrentCommand, con) == false)
        {
            DisplayAlert("Error", "Some items are not available, please put in your info and place a deposit.", "OK");
            Dictionary<string, object> updateInStock = new Dictionary<string, object>();
            updateInStock.Add("instock", "0");
            this.commande.Update(updateInStock);
            this.commande.Save();
        }
		else
		{
			DisplayAlert("Success", "All items are available", "OK");
			Navigation.PushAsync(new ChoicebtwPage());
		}
    }
    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        Dictionary<string, object> infoClient = new Dictionary<string, object>();
        string nom = customerData.Nom.Text;
        string prenom = customerData.Prenom.Text;
        string tel = customerData.Telephone.Text;

        if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(tel))
        {
            await DisplayAlert("Error", "All fields must be filled out.", "OK");
            return;
        }

        infoClient.Add("nom", nom);
        infoClient.Add("prenom", prenom);
        infoClient.Add("tel", tel);

        this.commande.Update(infoClient);
        this.commande.Save();

        await DisplayAlert("Success", "Your order has been successfully registered", "OK");
        await Navigation.PushAsync(new ChoicebtwPage());
    }

}
