using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DevTools;
using System.Data ; 
namespace KitBox.Views;

public partial class Modify_Price_Page : ContentPage
{
    private Connection con; 
    private object cle;
    public Modify_Price_Page(string oldselling_price, string code)
    {
        InitializeComponent();

        OLDselling_price.Text = oldselling_price;
        
        Connection.TestConnection();
        con = new Connection();
        cle = code;
    }

    private void ConfirmButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            
            string newselling_price = OLDselling_price.Text;
        

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


    