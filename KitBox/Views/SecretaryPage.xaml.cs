using DAL;
using Org.BouncyCastle.Utilities;
using Microsoft.Maui.Controls;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;

namespace KitBox.Views;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using DAL;
using System;
using System.Data;
using DevTools;


public partial class SecretaryPage : ContentPage
{
    
    private Connection con;
    internal class itemsattributes
        {
            
            public string Nom { get; set; }
            
            public string Price { get; set; }
            public itemsattributes(string nom, string price)
            {
                Nom = nom; 
                Price = price;
            }
        }
    public SecretaryPage()
    {
        InitializeComponent();
        BindingContext = this;
        Connection.TestConnection();
        con = new Connection();
        
    }

    

    private async void Modifier_Clicked(object sender, EventArgs e)
    {
        await (Navigation.PushAsync(new Customer_catalog())); //Ceci ne constitue pas la version finale, c'est juste un test
    }
    //Black,galvanisé,marron,white 
    
}