using Microsoft.Extensions.Logging;
using DAL;
using DevTools;

namespace KitBox;


public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		//FOR DEBUGGING DELETE BEFORE MERGE
		Connection.TestConnection();
		Connection con = new Connection();
		Armoire arm = new Armoire(con);
		Displayer.DisplayData(arm.Load(1));
		Dictionary<string,object> dico = new Dictionary<string,object>();
		dico["largeur"] = 2;
		Displayer.DisplayData(arm.LoadAll(dico));
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
