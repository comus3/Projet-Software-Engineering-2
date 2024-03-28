using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using AppServices;
using DAL;
using KitBox.AppServices;
using Microsoft.Extensions.Logging;
using DevTools;

namespace KitBox.Views
{
    public partial class OrderPartsPage : ContentPage
    {
        private Connection con;
        private ObservableCollection<SupplierData> suppliers;
        private Picker supplierPicker; 
        private Entry quantityEntry; 
        private string cle;
        private DataTable data_2; 

        public OrderPartsPage(string code)
        {
            InitializeComponent();
            cle = code;

            // Initialize UI elements
            InitializeUI();

            // Load suppliers
            LoadSuppliers();
        }

        private void InitializeUI()
        {
            Title = "Order Parts";
            

            supplierPicker = new Picker 
            {
                Title = "Select Supplier",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 300 // Augmenter la largeur
            };

            quantityEntry = new Entry 
            {
                Placeholder = "Enter Quantity",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 300 // Augmenter la largeur
            };

            Button confirmButton = new Button
            {
                Text = "Confirm",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 200 // Augmenter la largeur
            };
            confirmButton.Clicked += ConfirmButton_Clicked;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(new Label { Text = "Supplier:", FontSize = 18, Margin = new Thickness(10), HorizontalOptions = LayoutOptions.Center });
            stackLayout.Children.Add(supplierPicker);
            stackLayout.Children.Add(new Label { Text = "Quantity:", FontSize = 18, Margin = new Thickness(10), HorizontalOptions = LayoutOptions.Center });
            stackLayout.Children.Add(quantityEntry);
            stackLayout.Children.Add(confirmButton);

            Content = new ScrollView
            {
                Content = stackLayout,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
        }

        private void LoadSuppliers()
        {
            con = new Connection();
            Supplier sup_affichage = new Supplier(con);
           
            Dictionary<string, object> dico = new Dictionary<string, object>();
            dico["id_piece"] = cle;  
            suppliers = new ObservableCollection<SupplierData>();
            RtSupplier rt_supp = new RtSupplier(con); 
            List<string> colonnes_2 = new List<string> {"id_supplier" };
            data_2 = rt_supp.LoadAll(dico, colonnes_2);
            
            List<DataTable> datalist = new List<DataTable>();
            foreach (DataRow row in data_2.Rows)
            {
                Dictionary<string, object> dicotte = new Dictionary<string, object>();
                dicotte["id_supplier"] = row.ItemArray[0];
                datalist.Add(sup_affichage.LoadAll(dicotte));
            }

            foreach (DataTable supplier_data in datalist)
            {
                foreach (DataRow row in supplier_data.Rows) // Iterate through supplier_data.Rows instead of data_2.Rows
                {
                    string nom = row["nom"].ToString();
                    string id = row["id_supplier"].ToString();
                    string displayName = $"{nom} or {id}";

                    suppliers.Add(new SupplierData()
                    {
                        nom = nom,
                        Id = id
                    });
                    supplierPicker.Items.Add(displayName);
                }
            }
        }

        private void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (supplierPicker.SelectedIndex != -1 && supplierPicker.SelectedIndex < suppliers.Count)
                {
                    SupplierData selectedSupplier = suppliers[supplierPicker.SelectedIndex];
                    string selectedSupplierId = selectedSupplier.Id;

                    int id_supp;
                    if (!int.TryParse(selectedSupplierId, out id_supp))
                    {
                        throw new ArgumentException("Supplier ID is not valid.");
                    }

                    string quantityText = quantityEntry.Text;
                    int quantity;
                    if (!int.TryParse(quantityText, out quantity) || quantity <= 0)
                    {
                        throw new ArgumentException("Quantity must be a positive integer.");
                    }

                    StockServices.ExecuteAutoCommand(cle, quantity, id_supp, con);
                    Navigation.PushAsync(new StockManagerPage());
                }
            }
            catch (Exception exception)
            {
                Logger.WriteToFile(exception);
                DisplayAlert("Error", exception.Message, "OK");
            }
        }

        private class SupplierData
        {
            public string nom { get; set; }
            public string Id { get; set; }
        }
    }
}
