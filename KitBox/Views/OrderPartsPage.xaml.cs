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
        private DataTable data;
        private ObservableCollection<SupplierData> suppliers;
        private Picker supplierPicker; 
        private Entry quantityEntry; 
        private string cle;

        public OrderPartsPage(string code)
        {
            InitializeComponent();
            cle = code;
            supplierPicker = new Picker 
            {
                Title = "Select Supplier",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            con = new Connection();
            Supplier sup_affichage = new Supplier(con);
            List<string> colonnes = new List<string> { "nom", "id_supplier" };
            data = sup_affichage.LoadAll(null, colonnes);

            suppliers = new ObservableCollection<SupplierData>();

            
            
            
                foreach (DataRow row in data.Rows)
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
            
            

            quantityEntry = new Entry 
            {
                Placeholder = "Enter Quantity",
               
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Button confirmButton = new Button
            {
                Text = "Confirm",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            confirmButton.Clicked += ConfirmButton_Clicked;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(supplierPicker);
            stackLayout.Children.Add(quantityEntry);
            stackLayout.Children.Add(confirmButton);

            Content = stackLayout;
        }

        private void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            
                try
                {
                    if (supplierPicker.SelectedIndex != -1)
                    {
                        SupplierData selectedSupplier = suppliers[supplierPicker.SelectedIndex];

                        string selectedSupplierName = selectedSupplier.nom;
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
                    }
                    else
                    {
                       
                        DisplayAlert("Error", "Please select a supplier.", "OK");
                    }
                }
                catch (Exception ex)
                {
                   
                    DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            


            
        }

        private class SupplierData
        {
            public string nom { get; set; }
            public string Id { get; set; }
        }
    }
}
