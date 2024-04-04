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
using DevTools;
namespace KitBox.Views
{
    
    
    
    public partial class InputArrivalPage : ContentPage
    {
        private DataTable data;
    
        private ObservableCollection<HistoriqueData> historiques ;

        private string Code;
        private Connection con;

        public InputArrivalPage()
        {
            InitializeComponent();

            con = new Connection();
        }


    }
    public class HistoriqueData
    {
        
        public string Code_supp  { get; set; }
       
        public string Date { get; set; }

        public string Quantity { get; set; }
        
        public string Price_piece { get; set; }
        
        public string Price_total { get; set; }
       
    }


}
 
    // private Connection con;
    // private DataTable data;
    // private ObservableCollection<commandeData> commandes;
    // private ObservableCollection<HistoriqueData> historiques ;
    //
    // public InsightPage()
    // {
    //     InitializeComponent();
    //     Connection.TestConnection();
    //     con = new Connection();
    //
    //     // Chargement des données pour myListView1
    //     Commande affichage_a = new Commande(con);
    //     List<string> colonnes = new List<string>
    //     {
    //         "id_commande",
    //         "date",
    //         "price"
    //     };
    //     data = affichage_a.LoadAll(null, colonnes);
    //     commandes = new ObservableCollection<commandeData>(); 
    //     foreach (DataRow row in data.Rows)
    //     {
    //         commandes.Add(new commandeData
    //         {
    //             Code = row["id_commande"].ToString(),
    //             Date = row["date"].ToString(),
    //             Price = row["price"].ToString(),
    //         });
    //     }
    //     myListView1.ItemsSource = commandes;
    //
    //     // Chargement des données pour myListView2
    //     HistoriqueCommande affichage_b = new HistoriqueCommande(con);
    //     List<string> colonnes_b = new List<string>
    //     {
    //         "id_commande",
    //         "quantite",
    //         "prix_piece",
    //         "prix_total",
    //         "id_supplier",  
    //         "date"
    //     };
    //     data = affichage_b.LoadAll(null, colonnes_b); // Utilisation de la variable affichage_b
    //     historiques = new ObservableCollection<HistoriqueData>(); 
    //     foreach (DataRow row in data.Rows)
    //     {
    //         historiques.Add(new HistoriqueData
    //         {
    //             Code_supp = row["id_supplier"].ToString(),
    //             Date = row["date"].ToString(),
    //             Quantity = row["quantite"].ToString(),
    //             Price_piece = row["prix_piece"].ToString(),
    //             Price_total = row["prix_total"].ToString()
    //         });
    //     }
    //     myListView2.ItemsSource = historiques; // Affectation de la variable historiques
    // }