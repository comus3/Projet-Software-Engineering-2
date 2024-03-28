using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppServices;
using DAL;
using Microsoft.Maui.Controls;

namespace KitBox.Views
{
   
    public partial class InputArrivalPage : ContentPage
    { private string Code;
        private Connection con; 
        public InputArrivalPage(string cle)
        {
            InitializeComponent();
            Code = cle;
            con = new Connection();
        }

        private void OnConfirmClicked(object sender, EventArgs e)
        {
            
            string quantityText = quantityEntry.Text;
            int quantity;


            int.TryParse(quantityText, out quantity);
            
            StockServices.InputStockArrival(Code, quantity,con);

            Navigation.PushAsync(new StockManagerPage());


        }
    }
}