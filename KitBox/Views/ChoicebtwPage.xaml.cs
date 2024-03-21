using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1;

namespace KitBox.Views;

public partial class ChoicebtwPage : ContentPage
{
    public ChoicebtwPage()
    {
        InitializeComponent();
    }

    private async void OnredirectionOrdermaker(object sender, EventArgs e)
    {

        await (Navigation.PushAsync(new OrderMakerPage())); 
    }
    private async void OnredirectionShopKeeper(object sender, EventArgs e)
    {

        await (Navigation.PushAsync(new ShopKeeperPage())); 
    }

    private async void OnredirectionCustomer(object sender, EventArgs e)
    {

        await (Navigation.PushAsync(new Customer_catalog()));
        
        
    }
    private async void SecretaryClicked(object sender, EventArgs e)
    {

        await (Navigation.PushAsync(new SecretaryPage()));
        
        
    }
    
}