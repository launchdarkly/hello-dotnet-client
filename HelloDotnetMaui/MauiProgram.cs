using LaunchDarkly.Hello;
using LaunchDarkly.Sdk.Client;
using LaunchDarkly.Sdk.Client.Interfaces;
using Microsoft.Extensions.Logging;

namespace HelloDotnetMaui;

public static class MauiProgram
{
	public static ILdClient client;

	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		if (string.IsNullOrEmpty(DemoParameters.MobileKey))
		{
			throw new ArgumentException("Mobile Key was not set. Set in DemoParameters.cs .");
		}
		else
		{
			client = LdClient.Init(
				Configuration.Default(DemoParameters.MobileKey, ConfigurationBuilder.AutoEnvAttributes.Enabled),
				DemoParameters.MakeDemoContext(),
				DemoParameters.SdkTimeout
			);
		}


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
