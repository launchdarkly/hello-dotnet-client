using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace LaunchDarkly.Xamarin.Example
{
    public class FeatureFlagAdapter : BaseAdapter<FeatureFlag>
    {
        LayoutInflater inflater;
        List<FeatureFlag> featureFlags;

        public FeatureFlagAdapter(Activity context, IEnumerable<FeatureFlag> featureFlags)
        {
            this.featureFlags = new List<FeatureFlag>(featureFlags);
            this.inflater = LayoutInflater.From(context);
        }

        public override FeatureFlag this[int position] => featureFlags[position];

        public override int Count => featureFlags.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            
            View view = convertView;
            if (view == null)
            {
                view = inflater.Inflate(Resource.Layout.list_item, parent, false);
                view.SetTag(Resource.Id.FeatureFlagKeyTextView, view.FindViewById(Resource.Id.FeatureFlagKeyTextView));
                view.SetTag(Resource.Id.FeatureFlagValueTextView, view.FindViewById(Resource.Id.FeatureFlagValueTextView));
            }

            TextView flagKeyTextView = (TextView)view.GetTag(Resource.Id.FeatureFlagKeyTextView);
            TextView flagValueTextView = (TextView)view.GetTag(Resource.Id.FeatureFlagValueTextView);

            FeatureFlag flag = featureFlags[position];
            flagKeyTextView.Text = flag.FlagKey;
            flagValueTextView.Text = flag.FlagValue.IsNull ? "" : flag.FlagValue.ToString();

            return view;
        }
    }
}
