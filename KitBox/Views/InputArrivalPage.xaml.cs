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
using AppServices;
using DAL;
using DevTools;

namespace KitBox.Views
{




    public partial class InputArrivalPage : ContentPage
    {
        private DataTable data;
        private ObservableCollection<HistoriqueData> historiques;
        private List<HistoriqueData> initiallyUncheckedItems;
        private HistoriqueCommande affichage_b;
        private List<string> colonnes_b;

        private string Code;
        private Connection con;

        public InputArrivalPage()
        {
            InitializeComponent();

            con = new Connection();
            HistoriqueCommande affichage_b = new HistoriqueCommande(con);
            List<string> colonnes_b = new List<string>
            {
                "id_commande",
                "quantite",
                "prix_piece",
                "prix_total",
                "id_supplier",
                "date",
                "completed",
                "piece"
                
            };
            data = affichage_b.LoadAll(null, colonnes_b); // Utilisation de la variable affichage_b
            historiques = new ObservableCollection<HistoriqueData>();
            initiallyUncheckedItems = new List<HistoriqueData>(); // Initialisation de la liste
            foreach (DataRow row in data.Rows)
            {
                var historiqueItem = new HistoriqueData
                {
                    Code_supp = row["id_supplier"].ToString(),
                    Date = row["date"].ToString(),
                    Quantity =Convert.ToInt32( row["quantite"]) ,
                    Price_piece = row["prix_piece"].ToString(),
                    Price_total = row["prix_total"].ToString(),
                    CommandeID = row["id_commande"].ToString(), 
                    PieceID = row["piece"].ToString(),
                    Completed = Convert.ToBoolean(row["completed"]),
                   
                };

                if (!historiqueItem.Completed) // Si l'élément n'est pas déjà cochée, ajoutez-le à la liste
                {
                    initiallyUncheckedItems.Add(historiqueItem);
                }
                
                historiques.Add(historiqueItem);
            }

            myListView2.ItemsSource = historiques; // Affectation de la variable historiques
            RefreshData();
        }
        private void RefreshData()
        {
            // Rechargez les données depuis la base de données
            con = new Connection();
            HistoriqueCommande affichage_b = new HistoriqueCommande(con);
            List<string> colonnes_b = new List<string>
            {
                "id_commande",
                "quantite",
                "prix_piece",
                "prix_total",
                "id_supplier",
                "date",
                "completed",
                "piece"
                
            };
            data = affichage_b.LoadAll(null, colonnes_b); // Utilisation de la variable affichage_b
            historiques = new ObservableCollection<HistoriqueData>();
            initiallyUncheckedItems = new List<HistoriqueData>(); // Initialisation de la liste
            foreach (DataRow row in data.Rows)
            {
                var historiqueItem = new HistoriqueData
                {
                    Code_supp = row["id_supplier"].ToString(),
                    Date = row["date"].ToString(),
                    Quantity = Convert.ToInt32(row["quantite"]),
                    Price_piece = row["prix_piece"].ToString(),
                    Price_total = row["prix_total"].ToString(),
                    CommandeID = row["id_commande"].ToString(),
                    PieceID = row["piece"].ToString(),
                    Completed = Convert.ToBoolean(row["completed"]),
                    
                };

                if (!historiqueItem.Completed) // Si l'élément n'est pas déjà cochée, ajoutez-le à la liste
                {
                    initiallyUncheckedItems.Add(historiqueItem);
                }

                historiques.Add(historiqueItem);
            }
            historiques = new ObservableCollection<HistoriqueData>(historiques.OrderBy(h => h.Completed));

            myListView2.ItemsSource = historiques; 
           
        }


        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.BindingContext is HistoriqueData historiqueData)
            {
                if (initiallyUncheckedItems.Contains(historiqueData)) // Vérifiez si l'élément était initialement non cochée
                {
                    try
                    {
                        HistoriqueCommande historiquecommande = new HistoriqueCommande(con);
                        Dictionary<string, object> infocommande = new Dictionary<string, object>();

                        infocommande["completed"] = true;
                        infocommande["id_commande"] = historiqueData.CommandeID;
                        historiquecommande.Update(infocommande);
                        historiquecommande.Save();
                        Logger.WriteToFile(historiqueData.Code_supp);
                        StockServices.InputStockArrival(historiqueData.PieceID, historiqueData.Quantity, con);
                        RefreshData();
                    }
                    catch (Exception exception)
                    {
                        Logger.WriteToFile(exception);
                        throw;
                    } 
                }
            }
        }


        public class HistoriqueData
        {
            public string Code_supp { get; set; }
            public string Date { get; set; }
            public int Quantity { get; set; }
            public string Price_piece { get; set; }
            public string Price_total { get; set; }
            public string CommandeID { get; set; }
            public bool Completed { get; set; }
            
            public string PieceID { get; set; }
            
            

            public bool CheckBoxEnabled => !Completed; // Adjusted condition accordingly
        }



    }
}