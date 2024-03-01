using DAL;
using Org.BouncyCastle.Utilities;
using Microsoft.Maui.Controls;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;

namespace KitBox.Views;

public partial class SecretaryPage : ContentPage
{
    
    private Connection con;
    
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