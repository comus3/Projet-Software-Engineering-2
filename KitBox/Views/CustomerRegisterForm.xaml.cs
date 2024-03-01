using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;

namespace KitBox.Views;

public partial class CustomerRegisterForm : ContentPage
{
	private Connection con;
	private Commande commande;
	object date;
	object price;
	private Entry nom{
		get{
			Entry entry = new Entry();
			entry.Placeholder = "Nom";
			return entry;
		}
	}
	private Entry prenom
	{
		get
		{
			Entry entry = new Entry();
			entry.Placeholder = "Prenom";
			return entry;
		}
	}
	private Entry telephone
	{
		get
		{
			Entry entry = new Entry();
			entry.Placeholder = "Telephone";
			return entry;
		}
	}
	private Entry email
	{
		get
		{
			Entry entry = new Entry();
			entry.Placeholder = "Email";
			return entry;
		}
	}
	public CustomerRegisterForm()
	{
		BindingContext = this;
		InitializeComponent();
		object commandePk = 1;
		Connection.TestConnection();
		con = new Connection();
		this.commande = new Commande(con);
		DataTable res = this.commande.Load(commandePk);
		this.date = res.Rows[0].ItemArray[1];
		this.price = res.Rows[0].ItemArray[5];
		BuildUI();
	}
	private class CustomerData
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
			Text = "Nom",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold
		});
		stackLayout.Children.Add(nom);
		stackLayout.Children.Add(new Label
		{
			Text = "Prenom",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold
		});
		stackLayout.Children.Add(prenom);
		stackLayout.Children.Add(new Label
		{
			Text = "Telephone",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold
		});
		stackLayout.Children.Add(telephone);
		stackLayout.Children.Add(new Label
		{
			Text = "Email",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold
		});
		stackLayout.Children.Add(email);
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
		CustomerData customerdata = new CustomerData(){
			Nom = nom,
			Prenom = prenom,
			Telephone = telephone,
			Email = email
		};
		infoClient.Add("nom", customerdata.Nom.Text);
		infoClient.Add("prenom", customerdata.Prenom.Text);
		infoClient.Add("tel", customerdata.Telephone.Text);
		this.commande.Update(infoClient);
		this.commande.Save();
		DisplayAlert("Success", "Your order has been registered", "OK");
	}
}
