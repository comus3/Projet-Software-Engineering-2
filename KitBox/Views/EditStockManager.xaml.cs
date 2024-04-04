using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using DAL;
using DevTools;
using KitBox.Views;

namespace KitBox.Views
{
	public partial class EditStockManager : ContentPage
	{
		private Connection con;

		private DataTable data;

		private ObservableCollection<EditData> editData;

		public EditStockManager()
		{
			InitializeComponent();

			Connection.TestConnection();

			con = new Connection();


		}


		public class EditData
		{

			public string Stock { get; set; }

		}
	}
}