using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitBox.Views;

public partial class ChoicebtwPage : ContentPage
{
    public ChoicebtwPage()
    {
        InitializeComponent();
    }

    private async void OnredirectionOrdermaker(object sender, EventArgs e)
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