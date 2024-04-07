using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using DAL;
using DevTools;

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

        private void Add_clicked(object sender, EventArgs e)
        {
            
            
            addSupplierLayout.IsVisible = true;
            ButtonAdd.IsVisible = false; 
        }
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            
            
            addSupplierLayout.IsVisible = false;
            ButtonAdd.IsVisible = true; 
            nomEntry.Text = ""; 
            telephoneEntry.Text = "";
            adresseEntry.Text = "";
                
        }

            private void Save_Clicked(object sender, EventArgs e)
        {
        
        if (string.IsNullOrWhiteSpace(nomEntry.Text) || string.IsNullOrWhiteSpace(telephoneEntry.Text) || string.IsNullOrWhiteSpace(adresseEntry.Text))
        {
            
            DisplayAlert("Erreur", "Make sure to fill everything in", "OK");
            return; 
        }

        
        var supplier_data = new SupplierData();
        supplier_data.nom = nomEntry.Text;
        supplier_data.adresse = adresseEntry.Text;
        supplier_data.telephone = telephoneEntry.Text;
        
        try
        {
           
            Supplier supplier = new Supplier(con);
            Dictionary<string, object> infosupplier = new Dictionary<string, object>();
            infosupplier["nom"] = supplier_data.nom;
            infosupplier["adresse"] = supplier_data.adresse;
            infosupplier["telephone"] = supplier_data.telephone; 
            supplier.Update(infosupplier);
            supplier.Insert();
            
           
            RefreshPage();
        }
        catch (Exception ex)
        {
            
            DisplayAlert("Error", $" : {ex.Message}", "OK");
        }

       
        addSupplierLayout.IsVisible = false;
        ButtonAdd.IsVisible = true; 
        }

    private void RefreshPage() //en gros ça reactualise la page et en refastant de quoi faire la liste view 
    {
       
        nomEntry.Text = string.Empty;
        telephoneEntry.Text = string.Empty;
        adresseEntry.Text = string.Empty;

        
        Supplier sup_affichage = new Supplier(con);
        List<string> colonnes = new List<string>
        {
            "nom",
            "telephone",
            "adresse",
            "id_supplier"               
        };
        data = sup_affichage.LoadAll(null, colonnes);

        suppliers.Clear(); 
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
    }



        private void Details_Clicked(object sender, EventArgs e)
        {
            var supplier = (SupplierData)((ViewCell)sender).BindingContext;
            Navigation.PushAsync(new Suppliersitems(supplier.id_supplier, supplier.nom));

                         
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
