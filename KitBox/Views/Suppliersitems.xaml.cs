using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using DAL;
using Microsoft.Maui.Controls;
using AppServices;
using DevTools;
using KitBox.AppServices;
using Microsoft.Extensions.Logging;

namespace KitBox.Views
{
    public partial class Suppliersitems : ContentPage
    {
        private Connection con;
        private object cle;
        private ObservableCollection<Suppitemsdata> suppliers_items;
        private DataTable data;
        private string Name;

        private string selectedPartCode;

        public ICommand SearchCommand { get; private set; }

        public Suppliersitems(object code, object name, string select = null)
        {
            InitializeComponent();
            cle = code;

            con = new Connection();
            Connection.TestConnection();
            RtSupplier suppliersitems = new RtSupplier(con);
            Name = name.ToString();
            titre.Text = Name;
            List<string> colonnes = new List<string>
            {
                "id_relation",
                "id_piece",
                "delay_supplier",
                "price_supplier",
            };
            Dictionary<string, object> valuestoload = new Dictionary<string, object>();
            valuestoload["id_supplier"] = code;
            data = suppliersitems.LoadAll(valuestoload, colonnes);
            suppliers_items = new ObservableCollection<Suppitemsdata>();
            foreach (DataRow row in data.Rows)
            {
                suppliers_items.Add(new Suppitemsdata()
                {
                    Reference = row["id_piece"].ToString(),
                    Delay = row["delay_supplier"].ToString(),
                    Price = row["price_supplier"].ToString(),
                });
            }
            myListView.ItemsSource = suppliers_items;
            SearchCommand = new Command<string>(Search);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Vérifie si une pièce a été sélectionnée
            if (!string.IsNullOrEmpty(FetchingServices.SelectedPartCode))
            {
                // Affiche le libellé de la pièce sélectionnée
                selectedPieceLabel.Text = "Selected Piece: " + FetchingServices.SelectedPartCode;
                selectStackLayout.IsVisible = true;
            }
            else
            {
                // Cache le libellé s'il n'y a pas de pièce sélectionnée
                selectedPieceLabel.Text = string.Empty;
                selectStackLayout.IsVisible = false;
            }
        }

        private void Select_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SelectPartPage());
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            addSupplierLayout.IsVisible = true;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                selectedPartCode = FetchingServices.SelectedPartCode;
                Logger.WriteToFile(selectedPartCode);
                string newprice = price.Text?.ToString(); // Vérification pour null
                string newdelay = delay.Text?.ToString(); // Vérification pour null

                if (string.IsNullOrEmpty(selectedPartCode) || string.IsNullOrEmpty(newprice) || string.IsNullOrEmpty(newdelay))

                {
                    await DisplayAlert("Erreur", "Make sure to fill every field in .", "OK");
                    return;
                }

                RtSupplier rtt_supplier = new RtSupplier(con);
                Dictionary<string, object> infosupplier = new Dictionary<string, object>();

                infosupplier["price_supplier"] = newprice;
                Logger.WriteToFile(newprice);
                infosupplier["id_supplier"] = cle;
                Logger.WriteToFile(cle);
                infosupplier["delay_supplier"] = newdelay;
                Logger.WriteToFile(newdelay);
                infosupplier["id_piece"] = selectedPartCode;
                Logger.WriteToFile(selectedPartCode);
                rtt_supplier.Update(infosupplier);
                rtt_supplier.Insert();

                addSupplierLayout.IsVisible = false;
                selectStackLayout.IsVisible = false;

                FetchingServices.SelectedPartCode = null;

                await DisplayAlert("Success", "New piece added successfully!", "OK");

                // Rafraîchir la page
                RefreshPage();
            }
            catch (Exception exception)
            {
                Logger.WriteToFile(exception);
                await DisplayAlert("Error", $"An error occurred: {exception.Message}", "OK");
            }
        }

        private void RefreshPage()
        {
            // Clear existing data
            suppliers_items.Clear();

            // Reload data from the database
            RtSupplier suppliersitems = new RtSupplier(con);
            List<string> colonnes = new List<string>
            {
                "id_relation",
                "id_piece",
                "delay_supplier",
                "price_supplier",
            };
            Dictionary<string, object> valuestoload = new Dictionary<string, object>();
            valuestoload["id_supplier"] = cle;
            data = suppliersitems.LoadAll(valuestoload, colonnes);

            // Populate ObservableCollection with new data
            foreach (DataRow row in data.Rows)
            {
                suppliers_items.Add(new Suppitemsdata()
                {
                    Reference = row["id_piece"].ToString(),
                    Delay = row["delay_supplier"].ToString(),
                    Price = row["price_supplier"].ToString(),
                });
            }

            // Refresh the ListView
            myListView.ItemsSource = suppliers_items;
        }

        private void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                myListView.ItemsSource = suppliers_items;
            }
            else
            {
                // Filter the items based on the search query
                var filteredItems = suppliers_items.Where(s =>
                    s.Reference.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    s.Delay.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    s.Price.Contains(query, StringComparison.OrdinalIgnoreCase));

                myListView.ItemsSource = filteredItems;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(e.NewTextValue);
        }

        private void edit_price_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var supplier_items2 = (Suppitemsdata)button.CommandParameter;
            Navigation.PushAsync(new EditItemsSuppPage(supplier_items2.Reference, supplier_items2.Delay, supplier_items2.Price, Name));
        }

        public class Suppitemsdata
        {
            public string Reference { get; set; }
            public string Delay { get; set; }
            public string Price { get; set; }
        }

        public class RtSupplier : Model
        {
            public RtSupplier(Connection connection) : base(connection)
            {
                tableName = "rt_supplier";
                primaryKey = "id_relation";
            }
        }
    }
}
