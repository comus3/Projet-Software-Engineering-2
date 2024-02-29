using Microsoft.Extensions.Logging;
using DAL;
using DevTools;
using System.Collections.Generic; // Importation pour Dictionary
using System.Data;

namespace KitBox // Pas besoin de point-virgule ici
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //FOR DEBUGGING DELETE BEFORE MERGE
            
			Connection.TestConnection();
            /*
            Connection con = new Connection();

            Commande com = new Commande(con);
            Dictionary<string, object> infoClient = new Dictionary<string, object>();
            infoClient["date"] = 240224;
            com.Update(infoClient);
            DataTable data = com.Insert();
            Displayer.DisplayData(data);
            Displayer.DisplayData(com.getLastPk());

            Armoire arm = new Armoire(con);
            Dictionary<string, object> infoArm = new Dictionary<string, object>();
            infoArm["longueur"] = 2;
            infoArm["largeur"] = 1;
            infoArm["commande"] = com.getLastPk().Rows[0].ItemArray[0];
            arm.Update(infoArm);
            arm.Insert();

            Casier cas = new Casier(con);
            Dictionary<string, object> infoCasier = new Dictionary<string, object>();
            infoCasier["couleur"] = "JAUNEEE";
            infoCasier["h"] = 3;
            infoCasier["porte"] = 0;
            infoCasier["armoire"] = this.armoirePk;
			arm.getLastPk().Rows[0].ItemArray[0];
            cas.Update(infoCasier);
            cas.Insert();
            */
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
