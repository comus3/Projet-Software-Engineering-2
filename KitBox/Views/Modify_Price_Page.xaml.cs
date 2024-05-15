using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DevTools;


namespace KitBox.Views
{
   public partial class Modify_Price_Page : ContentPage
{
    private Connection con;
    private object cle;
     private Entry oldSellingPriceEntry; // Renommé le champ de classe

    public Modify_Price_Page(string oldselling_price, string code)
    {
        InitializeComponent();

       oldSellingPriceEntry.Text = oldselling_price; // Utilisation du nouveau nom

        Connection.TestConnection();
        con = new Connection();
        cle = code;
    }

    private void ConfirmButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string newselling_price = oldSellingPrice.Text; 

            Piece piece_spec = new Piece(con);

            DataTable result = piece_spec.Load(cle);

            Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
            valuesToUpdate["selling_price"] = newselling_price;

            piece_spec.Update(valuesToUpdate);
            piece_spec.Save();

            Navigation.PushAsync(new SecretaryPage());
        }
        catch (Exception ex)
        {
            Logger.WriteToFile($"Error occurred while updating price: {ex.Message}");
        }
    }
}


}
