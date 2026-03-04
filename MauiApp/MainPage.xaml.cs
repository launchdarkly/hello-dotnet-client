using System.Text.Json;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Client;
using LaunchDarkly.Sdk.Client.Interfaces;

namespace HelloDotNetClient;

public partial class MainPage : ContentPage
{
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
        var settings = await LoadAppSettings();
        var resolvedMobileKey = GetMobileKey(settings);
        var resolvedFlagKey = GetFlagKey(settings);

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
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateUI(resolvedFlagKey, flagValue);
            });

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

    private async Task<Dictionary<string, string>> LoadAppSettings()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("appsettings.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                   ?? new Dictionary<string, string>();
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

    private string? GetMobileKey(Dictionary<string, string> settings)
    {
        if (settings.TryGetValue("MobileKey", out var settingsKey) && !string.IsNullOrEmpty(settingsKey)
            && settingsKey != "my-mobile-key")
        {
            return settingsKey;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            FlagLabel.Text = "LaunchDarkly mobile key is required.\n\nCopy appsettings.example.json to appsettings.json in Resources/Raw/ and set your mobile key. See the README for details.";
        });
        return null;
    }

    private string? GetFlagKey(Dictionary<string, string> settings)
    {
        if (settings.TryGetValue("FlagKey", out var settingsKey) && !string.IsNullOrEmpty(settingsKey))
        {
            return settingsKey;
        }

        if (!string.IsNullOrEmpty(flagKey))
        {
            return flagKey;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            FlagLabel.Text = "LaunchDarkly flag key is required.\n\nSet the FlagKey in appsettings.json. See the README for details.";
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

