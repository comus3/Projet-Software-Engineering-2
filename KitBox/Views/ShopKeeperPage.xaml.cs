using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitBox.Views;

public partial class ShopKeeperPage : ContentPage
{
    public ShopKeeperPage()
    {
        InitializeComponent();

        lstCommande.ItemsSource = new string[]
            {
                "Commande 1",
                "Commande 2",
                "Commande 3",
                "Commande 4",
                "Commande 5"
            };
    }
}