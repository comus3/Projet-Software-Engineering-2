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
            // Handle the logic for saving new prices and navigating back to SecretaryPage here

            // You can access the new prices from OldPriceSupplier1Entry.Text and OldPriceSupplier2Entry.Text
            string newselling_price = OLDselling_price.Text;
        

            Piece piece_spec = new Piece(con);
            // Charger l'objet pièce en utilisant la méthode Load
            DataTable result = piece_spec.Load(cle);
           
                // Si l'objet pièce a été chargé avec succès, procéder à la mise à jour des valeurs
                Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
                valuesToUpdate["selling_price"] = newselling_price;
                
                piece_spec.Update(valuesToUpdate);

                // Enregistrer les modifications dans la base de données en appelant la méthode Save
                piece_spec.Save();

                Navigation.PushAsync(new SecretaryPage()); // Update your data accordingly and navigate back to SecretaryPage
            
            
        }
        catch (Exception ex)
        {
            // Gérer l'exception en enregistrant les détails dans un fichier journal
            Logger.WriteToFile($"Error occurred while updating price: {ex.Message}");
           
        }
    }


}


    