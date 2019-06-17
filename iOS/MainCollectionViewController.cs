using System;
using UIKit;
using LaunchDarkly.Client;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace LaunchDarkly.Xamarin.iOS
{
    public partial class MainCollectionViewController : UICollectionViewController, IFeatureFlagListener
    {
        IList<FeatureFlag> _flags;
        ILdMobileClient client;      

        // enter your mobile key here
        public const string mobileKey = "";

        // change to or use the features flags your going to be testing with
        public const string featureFlagDefaultKey = "featureFlagThatDoesntExist";
        public const string int_feature_flag = "int-feature-flag";
        public const string bool_feature_flag = "boolean-feature-flag";
        public const string string_feature_flag = "string-feature-flag";
        public const string json_feature_flag = "json-feature-flag";

        // set to the user key you want to test with
        public const string user_key = "";


        public MainCollectionViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupClient();
            SetCollectionViewLayout();
        }

        void SetupClient()
        {
            var user  = User.WithKey(user_key);
            var timeSpan = TimeSpan.FromSeconds(10);
            client = LdClient.Init(mobileKey, user, timeSpan);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            LoadFlagsIndividually();
            RegisterFeatureFlags();
        }

        private void SetCollectionViewLayout()
        {
            var layout = CollectionView.CollectionViewLayout as UICollectionViewFlowLayout;
            layout.SectionInset = new UIEdgeInsets(10, 10, 10, 10);
            layout.ItemSize = new CoreGraphics.CGSize(View.Bounds.Width - 20, 120);
        }

        void RegisterFeatureFlags()
        {
            client.RegisterFeatureFlagListener(int_feature_flag, this);
            client.RegisterFeatureFlagListener(bool_feature_flag, this);
            client.RegisterFeatureFlagListener(string_feature_flag, this);
        }

        private void LoadFlagsIndividually()
        {
            var intFlagValue = client.IntVariation(int_feature_flag, 0);
            var intFlag = new FeatureFlag { FlagKey = int_feature_flag, FlagValue = intFlagValue };
            var boolFlagValue = client.BoolVariation(bool_feature_flag, false);
            var boolFlag = new FeatureFlag { FlagKey = bool_feature_flag, FlagValue = boolFlagValue };
            var stringFlagValue = client.StringVariation(string_feature_flag, String.Empty);
            var stringFlag = new FeatureFlag { FlagKey = string_feature_flag, FlagValue = stringFlagValue };
            var defaultFlagValue = client.FloatVariation(featureFlagDefaultKey, 0.0f);
            var defaultFlag = new FeatureFlag { FlagKey = featureFlagDefaultKey, FlagValue = defaultFlagValue };
            var jsonFlagValue = client.JsonVariation(json_feature_flag, null);
            var jsonFlag = new FeatureFlag { FlagKey = json_feature_flag, FlagValue = jsonFlagValue };

            _flags = new[] { intFlag, boolFlag, stringFlag, defaultFlag, jsonFlag };

            this.CollectionView.ReloadData();
        }

        #region IFeatureFlagListener
        public void FeatureFlagChanged(string featureFlagKey, JToken value)
        {
            InvokeOnMainThread(() => { 
                
                var flagFromKey = _flags.SingleOrDefault(p => p.FlagKey == featureFlagKey);
                if (flagFromKey != null)
                {
                    flagFromKey.FlagValue = value;
                    this.CollectionView.ReloadData();
                }
            });
        }

        public void FeatureFlagDeleted(string featureFlagKey)
        {
            InvokeOnMainThread(() => {
                
                var flagFromKey = _flags.SingleOrDefault(p => p.FlagKey == featureFlagKey);
                if (flagFromKey != null)
                {
                    _flags.Remove(flagFromKey);
                    this.CollectionView.ReloadData();
                }
            });
        }
        #endregion

        public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell("FeatureFlagCell", indexPath) as FeatureFlagCell;
            var flag = _flags[indexPath.Row];
            cell.FeatureFlagKey = flag.FlagKey;
            cell.FlagValue = flag.FlagValue == null ? "" : flag.FlagValue.ToString();
            return cell;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            if (_flags != null)
                return _flags.Count;

            return 0;
        }
    }

    public class FeatureFlag
    {
        public string FlagKey;
        public JToken FlagValue;
    }
}