using LaunchDarkly.Client;

namespace LaunchDarkly.Xamarin.Example
{
    public class FeatureFlag
    {
        public string FlagKey;

        // For this demo, we'll store all of the flag values using the general-purpose LdValue type, even though
        // we can always query flag values as a more specific type.
        public LdValue FlagValue;

        public override string ToString()
        {
            return string.Format("{{FlagKey}:{0}}", FlagValue);
        }
    }
}
