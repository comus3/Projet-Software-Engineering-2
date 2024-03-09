using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DevTools;

namespace KitBox.Views;

public partial class Modify_Price_Page : ContentPage
{
    public Modify_Price_Page(string oldPriceSupplier1, string oldPriceSupplier2)
    {
        InitializeComponent();

        OldPriceSupplier1Entry.Text = oldPriceSupplier1;
        OldPriceSupplier2Entry.Text = oldPriceSupplier2;
    }

    private void ConfirmButton_Clicked(object sender, EventArgs e)
    {
        // Handle the logic for saving new prices and navigating back to SecretaryPage here
        // You can access the new prices from OldPriceSupplier1Entry.Text and OldPriceSupplier2Entry.Text
        string newPriceSupplier1 = OldPriceSupplier1Entry.Text;
        string newPriceSupplier2 = OldPriceSupplier2Entry.Text;
        
        Navigation.PushAsync(new SecretaryPage()); // Update your data accordingly and navigate back to SecretaryPage

    }

}


    