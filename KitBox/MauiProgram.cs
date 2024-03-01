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
            //FOR DEBUGGING DELETE BEFORE RELEASE
			Connection.TestConnection();
            //END OF DEBUGGING
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
