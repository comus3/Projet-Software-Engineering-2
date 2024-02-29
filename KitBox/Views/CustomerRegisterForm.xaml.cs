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
	private Entry nom;
	private Entry prenom;
	private Entry telephone;
	private Entry email;
	public CustomerRegisterForm(object commandePk)
	{
		BindingContext = this;
		Connection.TestConnection();
		con = new Connection();
		commande = new Commande(con);
		DataTable res = commande.Load(commandePk);
		object date = res.Rows[0].ItemArray[1];
		object price = res.Rows[0].ItemArray[5];
		BuildUI();
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
		infoClient.Add("nom", nom.Text);
		infoClient.Add("prenom", prenom.Text);
		infoClient.Add("telephone", telephone.Text);
		infoClient.Add("email", email.Text);
		commande.Update(infoClient);
		DisplayAlert("Success", "Your order has been registered", "OK");
	}
}
