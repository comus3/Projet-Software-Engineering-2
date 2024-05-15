using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;
using KitBox.AppServices;

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
			FontAttributes = FontAttributes.Bold
		};
		submit.Clicked += OnSubmitClicked;
		stackLayout.Children.Add(submit);
		Content = stackLayout;
	}
	private void OnSubmitClicked(object sender, EventArgs e)
	{
		Dictionary<string, object> infoClient = new Dictionary<string, object>();
		infoClient.Add("nom", customerData.Nom.Text);
		infoClient.Add("prenom", customerData.Prenom.Text);
		infoClient.Add("tel", customerData.Telephone.Text);
		this.commande.Update(infoClient);
		this.commande.Save();
		DisplayAlert("Success", "Your order has been successfully registered", "OK");
	}
}
