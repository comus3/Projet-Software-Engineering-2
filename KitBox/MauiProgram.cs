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
