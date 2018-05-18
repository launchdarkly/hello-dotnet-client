using Foundation;
using System;
using UIKit;

namespace LaunchDarkly.Xamarin.iOS
{
    public partial class FeatureFlagCell : UICollectionViewCell
    {
        public FeatureFlagCell (IntPtr handle) : base (handle)
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.Layer.CornerRadius = 10;
        }

        string featureFlagKey;
        public string FeatureFlagKey
        {
            get
            {
                return featureFlagKey;
            }

            set
            {
                featureFlagKey = value;
                label_FeatureFlagKey.Text = featureFlagKey;
            }
        }

        string flagValue;
        public string FlagValue
        {
            get
            {
                return flagValue;
            }

            set
            {
                flagValue = value;
                label_FlagValue.Text = flagValue;
            }
        }
    }
}