using System;
using UIKit;
using LaunchDarkly.Sdk.Client;
using LaunchDarkly.Sdk.Client.Interfaces;

namespace LaunchDarkly.Hello
{
    // This is the iOS version of the LaunchDarkly Xamarin SDK demo. The non-platform-specific
    // classes DemoMessages and DemoParameters are defined in ../Shared.

    // This file implements the application UI and demonstrates usage of the LaunchDarkly SDK.

    public partial class MainCollectionViewController : UIViewController
    {
        ILdClient client;

        public MainCollectionViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (string.IsNullOrEmpty(DemoParameters.MobileKey))
            {
                SetMessage(DemoMessages.MobileKeyNotSet);
            }
            else
            {
                client = LdClient.Init(
                    DemoParameters.MobileKey,
                    DemoParameters.MakeDemoUser(),
                    DemoParameters.SDKTimeout
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

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        void SetMessage(string s)
        {
            MessageView.Text = s;
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