using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppServices;
using DAL;
namespace KitBox.Views;

public partial class StockmanagerPage : ContentPage
{
    private Connection connection; 
    
    public StockmanagerPage()
    {
       // InitializeComponent();
        connection = new Connection();
        

    }
    
    
    
}