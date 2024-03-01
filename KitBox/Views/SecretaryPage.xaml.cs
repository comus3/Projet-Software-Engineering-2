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
    
    
}