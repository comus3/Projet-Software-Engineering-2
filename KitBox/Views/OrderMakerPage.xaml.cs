using DAL;
using System.Data;
using DevTools;
using AppServices;
using KitBox.AppServices;
using System.Collections.ObjectModel;
using Mysqlx.Crud;

namespace KitBox.Views;

public partial class OrderMakerPage : ContentPage
{
    private Connection conn;

    public class CommandeModel
    {
        public string IdCommande { get; set; }
    }

    public class PieceModel
    {
        public string CommandeId { get; set; }
        public string Reference { get; set; }
        public string Code { get; set; }
        public string DimensionsHauteur { get; set; }
        public string DimensionLargeur { get; set; }
        public string DimensionClient { get; set; }
        public string DimensionProfondeur { get; set; }
        public string DimensionDiametre { get; set; }
        public string DimensionLongueur { get; set; }
        public string Stock { get; set; }
        public string Reserve { get; set; }
        public string Await { get; set; }
        public string Type { get; set; }
        public string Selling_price { get; set; }
    }
    public ObservableCollection<CommandeModel> Commandes { get; set; } = new ObservableCollection<CommandeModel>();
    public ObservableCollection<PieceModel> Pieces { get; set; } = new ObservableCollection<PieceModel>();
  
    public OrderMakerPage()
    {
        InitializeComponent();
        BindingContext = this;

        // Testez la connexion
        Connection.TestConnection();
        // Initialisez la connexion
        conn = new Connection();

        LoadCommandes();
    }

    private void BackMenu(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ChoicebtwPage());
    }

        private void LoadCommandes()
        {
            Commande commande = new Commande(conn);
            Dictionary<string, object> com = new Dictionary<string, object>();
            
            List<string> colomns = new List<string>();
            colomns.Add("id_commande");
            colomns.Add("completed");
            colomns.Add("instock");
            DataTable data = commande.LoadAll(com,colomns);
            //Displayer.DisplayData(data);
            foreach (DataRow row in data.Rows)
            {
                if (row["completed"].ToString() == "False" && row["instock"].ToString() == "True")
                {
                    Commandes.Add(new CommandeModel { IdCommande = row["id_commande"].ToString() });
                }
            }
        //update puis save pour completed (quand boutton complet)
        }

        private void OnCompletedClicked(object sender, EventArgs e)
        {
            if(sender is Button button)
            {
                // Trouvez l'élément de commande associé au bouton cliqué
                var commandeModel = (CommandeModel)button.BindingContext;
                var idCommande = commandeModel.IdCommande;

                if(commandeModel != null && Commandes.Contains(commandeModel))
                {
                    var updateValues = new Dictionary<string, object>
                    {
                        { "completed", true } 
                    };

                    Commande commande = new Commande(conn);
                    commande.Load(idCommande);
                    commande.Update(updateValues);
                    commande.Save();

                    var piecesAssociees = Pieces.Where(p => p.CommandeId == commandeModel.IdCommande).ToList();
                    // Supprimez l'élément de la collection
                    Commandes.Remove(commandeModel);                    
                    foreach(var piece in piecesAssociees)
                    {
                        Pieces.Remove(piece);
                    }
                }
            }
        }

        private void Details(object sender, EventArgs e)
        {
            // Vérifiez si le sender est bien un bouton et récupérez le BindingContext
            if(sender is Button button && button.BindingContext is CommandeModel commandeModel)
            {
                // Récupérez l'ID de commande à partir du modèle lié au bouton
                var idCommande = commandeModel.IdCommande;

                // Liste des colonnes à récupérer pour les pièces
                // List<string> colomns = new List<string> 
                // {
                //     "Reference", "Code", "Dimensions_hauteur", "dimension_largeur", "dimension_client", 
                //     "dimension_profondeur", "dimension_diametre", "dimension_longeur", 
                //     "Stock", "Reserve", "Await", "Type", "Selling_price"
                // };

                // FetchCommandePieces pour récupérer seulement les pièces liées à cet ID de commande
                var piecesDataTables = FetchingServices.FetchCommandePieces(conn, idCommande);
                Pieces.Clear();

                foreach (var keyValuePair in piecesDataTables)
                {
                    foreach (DataRow row in keyValuePair.Value.Rows)
                    {
                        Pieces.Add(new PieceModel
                        {
                            CommandeId = idCommande,
                            Reference = row["Reference"].ToString(),
                            Code = row["Code"].ToString(),
                            DimensionsHauteur = row["Dimensions_hauteur"].ToString(),
                            DimensionLargeur = row["dimension_largeur"].ToString(),
                            DimensionClient = row["dimension_client"].ToString(),    
                            DimensionProfondeur = row["dimension_profondeur"].ToString(),
                            DimensionDiametre = row["dimension_diametre"].ToString(),
                            DimensionLongueur = row["dimension_longeur"].ToString(),
                            Stock = row["stock"].ToString(),
                            Reserve = row["reserve"].ToString(),
                            Await = row["await"].ToString(),
                            Type = row["type"].ToString(),
                            Selling_price = row["selling_price"].ToString()                            
                        });
                    }
                }
            }
        }
}