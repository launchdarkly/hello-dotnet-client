﻿using Android.App;
using Android.Widget;
using Android.OS;

namespace LaunchDarkly.Xamarin.Example
{
    [Activity(Label = "LaunchDarkly.Xamarin", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private ILdClient client;
        private TextView messageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            messageView = FindViewById<TextView>(Resource.Id.MessageView);

            if (string.IsNullOrEmpty(LaunchDarklyParameters.MobileKey))
            {
                messageView.Text = ExampleMessages.MobileKeyNotSet;
            }
            else
            {
                client = LdClient.Init(
                    // These values are set in the Shared project
                    LaunchDarklyParameters.MobileKey,
                    LaunchDarklyParameters.DefaultUser,
                    LaunchDarklyParameters.SDKTimeout
                );
                if (client.Initialized)
                {
                    UpdateFlagValue();
                    client.FlagChanged += FeatureFlagChanged;
                }
                else
                {
                    messageView.Text = ExampleMessages.InitializationFailed;
                }
            }
        }

        void UpdateFlagValue()
        {
            var flagValue = client.BoolVariation(LaunchDarklyParameters.FlagKey, false);
            messageView.Text = string.Format(ExampleMessages.FlagValueIs, flagValue);
        }

        void FeatureFlagChanged(object sender, FlagChangedEventArgs args)
        {
            if (args.Key == LaunchDarklyParameters.FlagKey)
            {
                UpdateFlagValue();
            }
        }
    }
}