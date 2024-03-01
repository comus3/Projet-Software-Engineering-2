using DAL;
using System.Data;
using DevTools;
using AppServices;
using KitBox.AppServices;

namespace KitBox.Views;

public partial class ShopKeeperPage : ContentPage
{
    private Connection conn;

    public ShopKeeperPage()
    {
        InitializeComponent();
        BindingContext = this;

        // Testez la connexion
        Connection.TestConnection();
        // Initialisez la connexion
        conn = new Connection();

        // Chargez les commandes depuis la base de données
        Commande commande = new Commande(conn);
        Dictionary<string, object> com = new Dictionary<string, object>();
        List<string> colomns = new List<string>();
        colomns.Add("id_commande");
        DataTable data = commande.LoadAll(com,colomns);
        //Displayer.DisplayData(data);
        foreach(DataRow row in data.Rows)
        {
            foreach(KeyValuePair<string,DataTable> keyValuePair in FetchingServices.FetchCommandePieces(conn,row.ItemArray[0]))
            {
                //Displayer.DisplayData(keyValuePair.Value);
            }
        }
        //update puis save pour completed (quand boutton complet)
    

    }
}