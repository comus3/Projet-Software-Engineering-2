using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using DAL;
using AppServices ;


namespace KitBox.Views;

public partial class Suppliersitems : ContentPage
{

    private Connection con;
    private object cle; 
    private ObservableCollection<Suppitemsdata> suppliers_items;
    private DataTable data;
    private string Name;
    
        public ICommand SearchCommand { get; private set; }
    public Suppliersitems(object code,object name)
    {
        InitializeComponent();
        cle = code;
        Connection con = new Connection(); 
        Connection.TestConnection();
        RtSupplier suppliersitems = new RtSupplier(con);
        Name = name.ToString();
        titre.Text = Name ; 
        List<string> colonnes = new List<string>
        {
            "id_relation",
            "id_piece",
            "delay_supplier",
            "price_supplier",
               
               
        };
        Dictionary<string, object> valuestoload = new Dictionary<string, object>();
        valuestoload["id_supplier"] = code;
        data = suppliersitems.LoadAll(valuestoload, colonnes); 
        suppliers_items= new ObservableCollection<Suppitemsdata>();
        foreach (DataRow row in data.Rows)
        {
            suppliers_items.Add(new Suppitemsdata()
            {
                Reference = row["id_relation"].ToString(),
                Delay = row["delay_supplier"].ToString(),
                Price = row["price_supplier"].ToString(),
               
                   
            });
        }
        myListView.ItemsSource = suppliers_items;
        SearchCommand = new Command<string>(Search);
        

    }
    private void Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
                
            myListView.ItemsSource = suppliers_items;
        }
        else
        {
            // Filter the items based on the search query
            var filteredItems = suppliers_items.Where(s =>
                s.Reference.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                s.Delay.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                s.Price.Contains(query, StringComparison.OrdinalIgnoreCase));
               
            myListView.ItemsSource = filteredItems;
        }
    }
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        Search(e.NewTextValue);
    }
    private void edit_price_Clicked(object sender, EventArgs e)
    {

        var button = (Button)sender;
       
        var supplier_items2 = (Suppitemsdata)button.CommandParameter;
            
        Navigation.PushAsync(new EditItemsSuppPage(supplier_items2.Reference, supplier_items2.Delay,  supplier_items2.Price, Name));
    
          
    }
    public class Suppitemsdata
    {
        
        public string Reference { get; set; }
        public string Delay { get; set; }
        
        public string Price { get; set; }
        
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