using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace KitBox.Views;

public partial class ShopKeeperPage : ContentPage
{
    private Connection con;
    internal class CommandeAttributes
    {
        public string Index {get; set;}
        public CommandeAttributes(string index)
        {
            this.Index = index;
        }
    }

    public ShopKeeperPage()
    {
        InitializeComponent();

        Connection.TestConnection();
        con = new Connection();

        Commande commande = new Commande(con);
        Dictionary<string, object> com = new Dictionary<string, object>();
        // com["index"] = 1;
        DataTable data = commande.LoadAll(com);
        Console.WriteLine(data);
        // List<CommandeAttributes> lstCommande = new List<CommandeAttributes>();
        // foreach (DataRow row in data.Rows)
        // {
        //     string Longueur = row[1].ToString();
        //     string Profondeur = row[2].ToString();
        //     string Price = row[3].ToString();
        //     Logger.WriteToFile(Longueur+ "  " + Profondeur + "  " + Price);

        //     CommandeAttributes commandeAttributes = new CommandeAttributes(Index);
        //     Logger.WriteToFile(commandeAttributes);
        //     lstCommande.Add(commandeAttributes);
        // }
        // // Set the ItemsSource of the ListView to your list of ArmoireAttributes
        // this.lstCom.ItemsSource = lstCommande;

        //}

    }
}