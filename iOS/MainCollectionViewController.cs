using System;
using UIKit;
using LaunchDarkly.Client;
using System.Collections.Generic;
using System.Linq;

namespace LaunchDarkly.Xamarin.iOS
{
    public partial class MainCollectionViewController : UICollectionViewController
    {
        IList<FeatureFlag> _flags;
        ILdClient client;      

        // enter your mobile key here
        public const string mobileKey = "";

        // change to or use the features flags you're going to be testing with
        public const string featureFlagDefaultKey = "featureFlagThatDoesntExist";
        public const string intFeatureFlag = "int-feature-flag";
        public const string boolFeatureFlag = "boolean-feature-flag";
        public const string stringFeatureFlag = "string-feature-flag";
        public const string jsonFeatureFlag = "json-feature-flag";

        // set to the user key you want to test with
        public const string userKey = "";


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
            var user  = User.WithKey(userKey);
            var timeSpan = TimeSpan.FromSeconds(10);
            client = LdClient.Init(mobileKey, user, timeSpan);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            LoadFlagsIndividually();
            client.FlagChanged += FeatureFlagChanged;
        }

        private void SetCollectionViewLayout()
        {
            var layout = CollectionView.CollectionViewLayout as UICollectionViewFlowLayout;
            layout.SectionInset = new UIEdgeInsets(10, 10, 10, 10);
            layout.ItemSize = new CoreGraphics.CGSize(View.Bounds.Width - 20, 120);
        }

        private void LoadFlagsIndividually()
        {
            var intFlagValue = client.IntVariation(intFeatureFlag, 0);
            var intFlag = new FeatureFlag { FlagKey = intFeatureFlag, FlagValue = LdValue.Of(intFlagValue) };
            var boolFlagValue = client.BoolVariation(boolFeatureFlag, false);
            var boolFlag = new FeatureFlag { FlagKey = boolFeatureFlag, FlagValue = LdValue.Of(boolFlagValue) };
            var stringFlagValue = client.StringVariation(stringFeatureFlag, String.Empty);
            var stringFlag = new FeatureFlag { FlagKey = stringFeatureFlag, FlagValue = LdValue.Of(stringFlagValue) };
            var defaultFlagValue = client.FloatVariation(featureFlagDefaultKey, 0.0f);
            var defaultFlag = new FeatureFlag { FlagKey = featureFlagDefaultKey, FlagValue = LdValue.Of(defaultFlagValue) };
            var jsonFlagValue = client.JsonVariation(jsonFeatureFlag, LdValue.Null);
            var jsonFlag = new FeatureFlag { FlagKey = jsonFeatureFlag, FlagValue = jsonFlagValue };

            _flags = new[] { intFlag, boolFlag, stringFlag, defaultFlag, jsonFlag };

            this.CollectionView.ReloadData();
        }

        public void FeatureFlagChanged(object sender, FlagChangedEventArgs args)
        {
            var flagFromKey = _flags.SingleOrDefault(p => p.FlagKey == args.Key);
            if (flagFromKey != null)
            {
                if (args.FlagWasDeleted)
                {
                    _flags.Remove(flagFromKey);
                }
                else
                {
                    flagFromKey.FlagValue = args.NewValue;
                }
                this.CollectionView.ReloadData();
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell("FeatureFlagCell", indexPath) as FeatureFlagCell;
            var flag = _flags[indexPath.Row];
            cell.FeatureFlagKey = flag.FlagKey;
            cell.FlagValue = flag.FlagValue.IsNull ? "" : flag.FlagValue.ToString();
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

        // For this demo, we'll store all of the flag values using the general-purpose LdValue type, even though
        // we can always query flag values as a more specific type.
        public LdValue FlagValue;
    }
}