using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;
using System.Collections.ObjectModel;

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
		DataTable res = commande.LoadAll(new Dictionary<string, object>{});
		Logger.WriteToFile(res.ToString());
		BuildUI(res);
	}
	private void BuildUI(DataTable res)
	{
		StackLayout stackLayout = new StackLayout();
		ScrollView scrollView = new ScrollView();
		scrollView.Content = stackLayout;
		var headerGrid = new Grid
		{
			HorizontalOptions = LayoutOptions.FillAndExpand,
			ColumnSpacing = 2
		};

		// Ajouter les en-têtes de colonnes au Grid
		for (int i = 0; i < res.Columns.Count; i++)
		{
			headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 150 });
			Label label = new Label { Text = res.Columns[i].ColumnName, FontAttributes = FontAttributes.Bold };
			Grid.SetColumn(label, i);
			headerGrid.Children.Add(label);
		}
		stackLayout.Children.Add(headerGrid);

		// Créer et ajouter les lignes de données
		foreach (DataRow row in res.Rows)
		{
			var rowGrid = new Grid
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				ColumnSpacing = 2
			};

			for (int i = 0; i < res.Columns.Count; i++)
			{
				rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 150 });
				var label = new Label { Text = row[i].ToString() };
				Grid.SetColumn(label, i); // Set the column position of the label
				rowGrid.Children.Add(label);
			}

			stackLayout.Children.Add(rowGrid);
		}
		Content = scrollView;
	}
}