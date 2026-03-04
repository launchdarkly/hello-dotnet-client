using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Client;
using LaunchDarkly.Sdk.Client.Interfaces;

namespace HelloDotNetClient;

public partial class MainPage : ContentPage
{
    // Set mobileKey to your LaunchDarkly mobile key.
    const string mobileKey = "";

    // Set flagKey to the feature flag key you want to evaluate.
    const string flagKey = "sample-feature";

    private LdClient? _client;

    public MainPage()
    {
        InitializeComponent();
        InitializeLaunchDarkly();
    }

    private async void InitializeLaunchDarkly()
    {
        var resolvedMobileKey = GetMobileKey();
        var resolvedFlagKey = GetFlagKey();

        if (resolvedMobileKey == null || resolvedFlagKey == null)
        {
            return; // Error message already shown
        }

        try
        {
            _client = await LdClient.InitAsync(
                Configuration.Default(resolvedMobileKey, ConfigurationBuilder.AutoEnvAttributes.Enabled),
                MakeContext(),
                TimeSpan.FromSeconds(10)
            );
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                FlagLabel.Text = $"SDK error: {ex.Message}";
            });
            return;
        }

        if (_client.Initialized)
        {
            var flagValue = _client.BoolVariation(resolvedFlagKey, false);
            UpdateUI(resolvedFlagKey, flagValue);

            _client.FlagTracker.FlagValueChanged += (sender, eventArgs) =>
            {
                if (eventArgs.Key == resolvedFlagKey)
                {
                    var newValue = _client.BoolVariation(resolvedFlagKey, false);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        UpdateUI(resolvedFlagKey, newValue);
                    });
                }
            };
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                FlagLabel.Text = "SDK failed to initialize. Please check your internet connection and SDK credential for any typo.";
            });
        }
    }

    private void UpdateUI(string flagKeyName, bool flagValue)
    {
        FlagLabel.Text = $"The {flagKeyName} feature flag evaluates to {flagValue.ToString().ToLowerInvariant()}.";
        Page.BackgroundColor = flagValue ? Color.FromArgb("#00844B") : Color.FromArgb("#373841");
    }

    private string? GetMobileKey()
    {
        if (!string.IsNullOrEmpty(mobileKey))
        {
            return mobileKey;
        }

        var envKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_MOBILE_KEY");
        if (!string.IsNullOrEmpty(envKey))
        {
            return envKey;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            FlagLabel.Text = "LaunchDarkly mobile key is required: set the mobileKey variable in MainPage.xaml.cs, or the LAUNCHDARKLY_MOBILE_KEY environment variable and try again.";
        });
        return null;
    }

    private string? GetFlagKey()
    {
        var envKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_FLAG_KEY");
        if (!string.IsNullOrEmpty(envKey))
        {
            return envKey;
        }

        if (!string.IsNullOrEmpty(flagKey))
        {
            return flagKey;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            FlagLabel.Text = "LaunchDarkly flag key is required: set the flagKey variable in MainPage.xaml.cs, or the LAUNCHDARKLY_FLAG_KEY environment variable and try again.";
        });
        return null;
    }

    // Set up the evaluation context. This context should appear on your
    // LaunchDarkly contexts dashboard soon after you run the demo.
    private static Context MakeContext() =>
        Context.Builder("example-user-key")
            .Name("Sandy")
            .Build();
}

