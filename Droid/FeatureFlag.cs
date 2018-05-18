using Newtonsoft.Json.Linq;

namespace LaunchDarkly.Xamarin.Droid
{
    public class FeatureFlag
    {
        public string FlagKey;
        public JToken FlagValue;

        public override string ToString()
        {
            return string.Format("{{FlagKey}:{0}}", FlagValue);
        }
    }
}

