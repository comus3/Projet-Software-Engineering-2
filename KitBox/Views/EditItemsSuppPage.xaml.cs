using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DevTools;
using System.Data ; 
using System.Collections.ObjectModel;
namespace KitBox.Views;

public partial class EditItemsSuppPage : ContentPage
{
    private Connection con;
    private object cle; 
    private DataTable data;
    private string supplier_name; 
    private string suppID ; 
    private string relid ; 
    public EditItemsSuppPage(string relation , string reference, string delay, string price, string name, string id_supplier)
    {
        InitializeComponent();
        con = new Connection();
        Connection.TestConnection();
        cle = reference;
        OLDDelay.Text = delay;
        OLDPrice.Text = price;
        supplier_name = name; 
        suppID = id_supplier;
        relid = relation ;  
    }
    private void ConfirmButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string newdelay = OLDDelay.Text;
            string newprice = OLDPrice.Text;
        
            RtSupplier rt_supplier = new RtSupplier(con);
       
            DataTable result = rt_supplier.Load(relid);
       
            Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
            valuesToUpdate["delay_supplier"] = newdelay;
            valuesToUpdate["price_supplier"] = newprice;
        
            rt_supplier.Update(valuesToUpdate);

            rt_supplier.Save();

           
            
            // Navigate to Suppliersitems page with necessary parameters
            Navigation.PushAsync(new Suppliersitems(suppID, supplier_name));
        }
        catch (Exception ex)
        {
            Logger.WriteToFile($"Error occurred while updating price: {ex.Message}");
            throw;
        }
    }

    public class RtSupplier : Model
    {

        public RtSupplier(Connection connection) : base(connection)
        {
            tableName = "rt_supplier";
            primaryKey = "id_relation";
        }
    }
    
}