using DAL;
using System.Data;
using DevTools;
using AppServices;
using KitBox.AppServices;
using System.Collections.ObjectModel;

namespace KitBox.Views;

public partial class ShopKeeperPage : ContentPage
{
    private Connection conn;

    public class CommandeModel
    {
        public string IdCommande { get; set; }
    }
    public ObservableCollection<CommandeModel> Commandes { get; set; } = new ObservableCollection<CommandeModel>();
        
    public ShopKeeperPage()
    {
        InitializeComponent();
        BindingContext = this;

        // Testez la connexion
        Connection.TestConnection();
        // Initialisez la connexion
        conn = new Connection();

        LoadCommandes();

        

        // foreach (DataRow row in data.Rows)
        // {
        //     //myListBox.Items.Add(data.GetString);
        //     //string numero = row.ToString();
        // }

        // foreach(DataRow row in data.Rows)
        // {
        //     foreach(KeyValuePair<string,DataTable> keyValuePair in FetchingServices.FetchCommandePieces(conn,row.ItemArray[0]))
        //     {
        //         //Displayer.DisplayData(keyValuePair.Value);
        //     }
        // }
    }

        private void LoadCommandes()
        {
            // Supposons que cette méthode récupère les données comme avant et les ajoute à Commandes
            // Exemple simplifié:
            // Chargez les commandes depuis la base de données
            Commande commande = new Commande(conn);
            Dictionary<string, object> com = new Dictionary<string, object>();
            List<string> colomns = new List<string>();
            colomns.Add("id_commande");
            DataTable data = commande.LoadAll(com,colomns);
            Displayer.DisplayData(data);
            //DataTable data = ... // Récupérez les données comme dans votre exemple
            foreach (DataRow row in data.Rows)
            {
                Commandes.Add(new CommandeModel { IdCommande = row["id_commande"].ToString() });
            }
        //update puis save pour completed (quand boutton complet)
        }

        // private void Details(object sender, EventArgs e)
        // {
        //     Commande commande = new Commande(conn);
        //     Dictionary<string, object> com = new Dictionary<string, object>();
        //     List<string> colomns = new List<string>();
        //     colomns.Add("id_commande");
        //     DataTable data = commande.LoadAll(com,colomns);
        //     foreach(DataRow row in data.Rows)
        //     {
        //         foreach(KeyValuePair<string,DataTable> keyValuePair in FetchingServices.FetchCommandePieces(conn,row.ItemArray[0]))
        //         {
        //             Displayer.DisplayData(keyValuePair.Value);
        //         }
        //     }

        // }
}