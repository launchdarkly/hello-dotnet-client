using LaunchDarkly.Hello;
using LaunchDarkly.Sdk.Client.Interfaces;

namespace HelloDotnetMaui;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (MauiProgram.client.Initialized)
		{
			UpdateFlagValue();
			MauiProgram.client.FlagTracker.FlagValueChanged += FeatureFlagChanged;
		}
		else
		{
			SetMessage(DemoMessages.InitializationFailed);
		}
	}

	void SetMessage(string s)
	{
		MessageTxt.Text = s;
	}

	void UpdateFlagValue()
	{
		var flagValue = MauiProgram.client.BoolVariation(DemoParameters.FeatureFlagKey, false);
		if (flagValue)
		{
			MainPageLayout.BackgroundColor = Application.Current.Resources["BackgroundTrue"] as Color;
		}
		else
		{
			MainPageLayout.BackgroundColor =  Application.Current.Resources["BackgroundFalse"] as Color;
		}
		SetMessage(string.Format(DemoMessages.FlagValueIs, DemoParameters.FeatureFlagKey, flagValue));
	}

	void FeatureFlagChanged(object sender, FlagValueChangeEvent args)
	{
		if (args.Key == DemoParameters.FeatureFlagKey)
		{
			UpdateFlagValue();
		}
	}
}

