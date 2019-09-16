using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using LaunchDarkly.Client;
using System;
using System.Linq;

namespace LaunchDarkly.Xamarin.Droid
{
    [Activity(Label = "LaunchDarkly.Xamarin", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private IList<FeatureFlag> _flags;
        private ListView ListView;
        private ILdClient client;

        // enter your mobile key here
        public const string mobileKey = "";

        // set to the user key you want to test with
        public const string userKey = "";

        // change to or use the features flags you're going to be testing with
        public const string featureFlagDefaultKey = "featureFlagThatDoesntExist";
        public const string intFeatureFlag = "int-feature-flag";
        public const string boolFeatureFlag = "boolean-feature-flag";
        public const string stringFeatureFlag = "string-feature-flag";
        public const string jsonFeatureFlag = "json-feature-flag";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ListView = FindViewById<ListView>(Resource.Id.MainListView);

            SetupClient();        
        }

        void SetupClient()
        {
            var user = User.WithKey(userKey);
            var timeSpan = TimeSpan.FromSeconds(10);
            client = LdClient.Init(mobileKey, user, timeSpan);
            LoadFeatureFlagRows();
            client.FlagChanged += FeatureFlagChanged;
        }

        void LoadFeatureFlagRows()
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

            ListView.Adapter = new FeatureFlagAdapter(this, _flags);
        }

        public void FeatureFlagChanged(object sender, FlagChangedEventArgs args)
        {
            var flagFromKey = _flags.Where(p => p.FlagKey == args.Key).FirstOrDefault();
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
                

                ReloadListViewAdapter();
            }
        }

        private void ReloadListViewAdapter()
        {
            var featureFlagAdapter = new FeatureFlagAdapter(this, _flags);
            var adapter = (BaseAdapter)featureFlagAdapter;
            adapter.NotifyDataSetChanged();
            ListView.Adapter = adapter;
        }
    }
}

