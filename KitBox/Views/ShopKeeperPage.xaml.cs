using KitBox.AppServices;
using System.Collections.ObjectModel;
using DAL;
using System.Data;
using KitBox.Views;

namespace MauiApp1;

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

        LoadCommandesInstock();
	}
	private void BackMenu(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ChoicebtwPage());
    }
	private void LoadCommandesInstock()
        {
            Commande commande = new Commande(conn);
            Dictionary<string, object> com = new Dictionary<string, object>();
            
            List<string> colomns = new List<string>();
            colomns.Add("id_commande");
            colomns.Add("completed");
			colomns.Add("payement");
            colomns.Add("instock");
            DataTable data = commande.LoadAll(com,colomns);
            //Displayer.DisplayData(data);
            foreach (DataRow row in data.Rows)
            {
                if (row["completed"].ToString() == "False" && row["payement"].ToString() == "False" && row["instock"].ToString() == "True")
                {
                    Commandes.Add(new CommandeModel { IdCommande = row["id_commande"].ToString() });
                }
            }
        //update puis save pour completed (quand boutton complet)
        }
	private void LoadCommandesNotStock()
	{
		Commande depositC = new Commande(conn);
		Dictionary<string, object> com = new Dictionary<string, object>();
            
            List<string> colomns = new List<string>();
            colomns.Add("id_commande");
            colomns.Add("completed");
			colomns.Add("payement");
            colomns.Add("instock");
            DataTable data = depositC.LoadAll(com,colomns);
            //Displayer.DisplayData(data);
            foreach (DataRow row in data.Rows)
            {
                if (row["completed"].ToString() == "False" && row["payement"].ToString() == "False" && row["instock"].ToString() == "False")
                {
                    depositC.Add(new CommandeModel { IdCommande = row["id_commande"].ToString() });
                }
            }
	}
	private void OnPayementClicked(object sender, EventArgs e){

	}
}