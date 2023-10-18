using Android.App;
using Android.Graphics;
using Android.Widget;
using Android.OS;
using LaunchDarkly.Sdk.Client;
using LaunchDarkly.Sdk.Client.Interfaces;
using ConfigurationBuilder = LaunchDarkly.Sdk.Client.ConfigurationBuilder;

namespace LaunchDarkly.Hello
{
    // This is the Android version of the LaunchDarkly client-side .NET SDK demo. The
    // non-platform-specific classes DemoMessages and DemoParameters are defined in ../Shared.

    // This file implements the application UI and demonstrates usage of the LaunchDarkly SDK.

    [Activity(Label = "LaunchDarkly.Hello", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private ILdClient client;
        private TextView messageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            messageView = FindViewById<TextView>(Resource.Id.MessageView);

            if (string.IsNullOrEmpty(DemoParameters.MobileKey))
            {
                SetMessage(DemoMessages.MobileKeyNotSet);
            }
            else
            {
                client = LdClient.Init(
                    // These values are set in the Shared project
                    Configuration.Default(DemoParameters.MobileKey, ConfigurationBuilder.AutoEnvAttributes.Enabled),
                    DemoParameters.MakeDemoContext(),
                    DemoParameters.SdkTimeout
                );
                if (client.Initialized)
                {
                    UpdateFlagValue();
                    client.FlagTracker.FlagValueChanged += FeatureFlagChanged;
                }
                else
                {
                    SetMessage(DemoMessages.InitializationFailed);
                }
            }
        }

        void SetMessage(string s)
        {
            messageView.Text = s;
        }

        void UpdateFlagValue()
        {
            var flagValue = client.BoolVariation(DemoParameters.FeatureFlagKey, false);
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
}
