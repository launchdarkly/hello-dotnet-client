using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using LaunchDarkly.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Android.Content;

namespace LaunchDarkly.Xamarin.Droid
{
    [Activity(Label = "LaunchDarkly.Xamarin", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, IFeatureFlagListener
    {
        private IList<FeatureFlag> _flags;
        private ListView ListView;
        private ILdMobileClient client;

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
            var user = User.WithKey(user_key);
            client = LdClient.Init(mobileKey, user);
            LoadFeatureFlagRows();
            RegisterFeatureFlags();
        }

        void LoadFeatureFlagRows()
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

            ListView.Adapter = new FeatureFlagAdapter(this, _flags);
        }

        void RegisterFeatureFlags()
        {
            client.RegisterFeatureFlagListener(int_feature_flag, this);
            client.RegisterFeatureFlagListener(bool_feature_flag, this);
            client.RegisterFeatureFlagListener(string_feature_flag, this);
            client.RegisterFeatureFlagListener(json_feature_flag, this);
        }

        #region IFeatureFlagListener
        public void FeatureFlagChanged(string featureFlagKey, JToken value)
        {
            var flagFromKey = _flags.Where(p => p.FlagKey == featureFlagKey).FirstOrDefault();
            if (flagFromKey != null)
            {
                flagFromKey.FlagValue = value;

                RunOnUiThread(() =>
                {
                    ReloadListViewAdapter();
                });
            }
        }

        public void FeatureFlagDeleted(string featureFlagKey)
        {
            var flagFromKey = _flags.Where(p => p.FlagKey == featureFlagKey).FirstOrDefault();
            if (flagFromKey != null)
            {
                _flags.Remove(flagFromKey);

                RunOnUiThread(() =>
                {
                    ReloadListViewAdapter();  
                });
            }
        }
        #endregion

        private void ReloadListViewAdapter()
        {
            var featureFlagAdapter = new FeatureFlagAdapter(this, _flags);
            var adapter = (BaseAdapter)featureFlagAdapter;
            adapter.NotifyDataSetChanged();
            ListView.Adapter = adapter;
        }
    }
}

