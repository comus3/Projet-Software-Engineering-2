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

    public class PieceModel
    {
        public string Reference { get; set; }
        public string Code { get; set; }
    }

    public ObservableCollection<CommandeModel> Commandes { get; set; } = new ObservableCollection<CommandeModel>();
    public ObservableCollection<PieceModel> Pieces { get; set; } = new ObservableCollection<PieceModel>();
  
    public ShopKeeperPage()
    {
        InitializeComponent();
        BindingContext = this;

        // Testez la connexion
        Connection.TestConnection();
        // Initialisez la connexion
        conn = new Connection();

        LoadCommandes();
    }

        private void LoadCommandes()
        {
            Commande commande = new Commande(conn);
            Dictionary<string, object> com = new Dictionary<string, object>();
            List<string> colomns = new List<string>();
            colomns.Add("id_commande");
            DataTable data = commande.LoadAll(com,colomns);
            Displayer.DisplayData(data);
            foreach (DataRow row in data.Rows)
            {
                Commandes.Add(new CommandeModel { IdCommande = row["id_commande"].ToString() });
            }
        //update puis save pour completed (quand boutton complet)
        }

        private void OnCompletedClicked(object sender, EventArgs e)
        {
            if(sender is Button button)
            {
                // Trouvez l'élément de commande associé au bouton cliqué
                var commandeModel = (CommandeModel)button.BindingContext;

                if(commandeModel != null && Commandes.Contains(commandeModel))
                {
                    // Supprimez l'élément de la collection
                    Commandes.Remove(commandeModel);
                    
                    // Mettez à jour la base de données ou effectuez d'autres actions nécessaires ici
                }
            }
        }



        private void Details(object sender, EventArgs e)
        {
            // Commande commande = new Commande(conn);
            // Dictionary<string, object> com = new Dictionary<string, object>();
            // List<string> colomns = new List<string>();
            // //colomns.Add("id_commande");
            // DataTable data = commande.LoadAll(com,colomns);
            // foreach(DataRow row in data.Rows)
            // {
            //     foreach(KeyValuePair<string,DataTable> keyValuePair in FetchingServices.FetchCommandePieces(conn,row.ItemArray[0]))
            //     {
            //         Displayer.DisplayData(keyValuePair.Value);
            //         foreach (DataColumn column in keyValuePair.Value.Columns)
            //         {
            //             Console.WriteLine(column.ColumnName);
            //         }
                    // Pieces.Add(new PieceModel { 
                    //     Reference = row["reference"].ToString(),
                    //     Code = row["code"].ToString(),

                    //      });
                    // Console.WriteLine(Pieces);
                // }
            // }

        }
}