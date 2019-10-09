using System;
using UIKit;
using LaunchDarkly.Client;
using System.Collections.Generic;
using System.Linq;

namespace LaunchDarkly.Xamarin.Example
{
    public partial class MainCollectionViewController : UIViewController
    {
        ILdClient client;

        public MainCollectionViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            
            if (string.IsNullOrEmpty(LaunchDarklyParameters.MobileKey))
            {
                SetMessage(ExampleMessages.MobileKeyNotSet);
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
                    SetMessage(ExampleMessages.InitializationFailed);
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
            var flagValue = client.BoolVariation(LaunchDarklyParameters.FlagKey, false);
            SetMessage(string.Format(ExampleMessages.FlagValueIs, flagValue));
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