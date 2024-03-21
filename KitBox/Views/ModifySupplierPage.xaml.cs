using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DevTools;
using System.Data ; 


namespace KitBox.Views;



public partial class ModifySupplierPage : ContentPage

{

    private Connection con;
    private object cle; 
   
    public ModifySupplierPage(string oldname, string oldtelephone, string oldadresse, string id_supplier)
    {
        InitializeComponent();
        con = new Connection();
        Connection.TestConnection();
         cle = id_supplier;
         OLDname.Text = oldname;
         OLDtelephone.Text = oldtelephone;
         OLDadresse.Text = oldadresse;

    }
    private void ConfirmButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            
            string newname = OLDname.Text;
            string newadresse = OLDadresse.Text;
            string newtelephone = OLDtelephone.Text;
            Supplier supplier = new Supplier(con);
           
            DataTable result = supplier.Load(cle);
           
               
            Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
            valuesToUpdate["nom"] = newname;
            valuesToUpdate["telephone"] = newtelephone;
            valuesToUpdate["adresse"] = newadresse;
            supplier.Update(valuesToUpdate);

               
            supplier.Save();

            Navigation.PushAsync(new SuppliersPage()); 
            
            
        }
        catch (Exception ex)
        {
            
            Logger.WriteToFile($"Error occurred while updating price: {ex.Message}");
           
        }
    }
    
    
}
