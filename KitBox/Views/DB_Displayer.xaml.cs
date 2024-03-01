using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;

namespace KitBox.Views;


public partial class DB_Displayer : ContentPage
{
	Connection con;
	int integer = 1;
	public DB_Displayer()
	{
		BindingContext = this;
		InitializeComponent();
		Connection.TestConnection();
		con = new Connection();
		Commande commande = new Commande(con);
		Dictionary<string, object> info = new Dictionary<string, object>{};
		info.Add("index", integer.ToString());
		info.Add("date", "240224");
		DataTable res = commande.LoadAll(info);
		Console.WriteLine(res);
		Console.WriteLine(res.Rows[0].ItemArray[0]);
		Console.WriteLine(info);
		BuildUI(res);
	}
	private void BuildUI(DataTable res)
	{
		StackLayout stackLayout = new StackLayout();
		foreach (DataRow row in res.Rows)
		{
			foreach (var item in row.ItemArray)
			{
				stackLayout.Children.Add(new Label
				{
					Text = item.ToString()+ "Test" + integer.ToString(),
					FontSize = 20,
					FontAttributes = FontAttributes.Bold
				});
			}
		}
		Content = stackLayout;
	}
}