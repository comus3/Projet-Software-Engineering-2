using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using DAL;

namespace KitBox.Views
{
    public partial class SuppliersPage : ContentPage
    {
        private Connection con;
        private DataTable data;
        private ObservableCollection<SupplierData> suppliers;

        public ICommand SearchCommand { get; private set; }

        public SuppliersPage()
        {
            InitializeComponent();
            Connection.TestConnection();
            con = new Connection();
            Supplier sup_affichage = new Supplier(con);
            List<string> colonnes = new List<string>
            {
                "nom",
                "telephone",
                "adresse",
                "id_supplier"               
               
            };
            data = sup_affichage.LoadAll(null, colonnes);

            
            suppliers = new ObservableCollection<SupplierData>();
            foreach (DataRow row in data.Rows)
            {
                suppliers.Add(new SupplierData()
                {
                    nom = row["nom"].ToString(),
                    adresse = row["adresse"].ToString(),
                    telephone = row["telephone"].ToString(),
                    id_supplier = row["id_supplier"].ToString()
                   
                });
            }

           
            myListView.ItemsSource = suppliers;

            
            SearchCommand = new Command<string>(Search);
        }

        private void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                
                myListView.ItemsSource = suppliers;
            }
            else
            {
                // Filter the items based on the search query
                var filteredItems = suppliers.Where(s =>
                    s.nom.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    s.adresse.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    s.telephone.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    s.id_supplier.Contains(query, StringComparison.OrdinalIgnoreCase));
                myListView.ItemsSource = filteredItems;
            }
        }
        private void edit_price_Clicked(object sender, EventArgs e)
        {
           
            var button = (Button)sender;
            var supplier = (SupplierData)button.CommandParameter;
            
            Navigation.PushAsync(new ModifySupplierPage(supplier.nom, supplier.telephone, supplier.adresse, supplier.id_supplier));
    
          
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(e.NewTextValue);
        }

        
        public class SupplierData
        {
            public string nom { get; set; }
            public string telephone { get; set; }
            public string adresse { get; set; }
            public string id_supplier { get; set; }
            
        }
    }
}
