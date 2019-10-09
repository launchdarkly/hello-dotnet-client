using System;
using LaunchDarkly.Client;

namespace LaunchDarkly.Xamarin.Example
{
    // These values are used by both the Android and the iOS versions of the demo.
    public class LaunchDarklyParameters
    {
        // Enter your mobile key here - the demo will not run without this
        public const string MobileKey = "mob-368413a0-28e1-495d-ab32-7aa389ac33b6";

        // Enter the key of a boolean feature flag in your LaunchDarkly project.
        public const string FlagKey = "test-flag";

        // Set to the user key you want to test with
        public const string UserKey = "test-user-key";

        // You may add any other desired user properties here
        public static readonly User DefaultUser = User.Builder(UserKey)
            // for instance: .Name("test-user-name")
            .Build();

        // How long the application will wait for the SDK to connect to LaunchDarkly
        public static TimeSpan SDKTimeout = TimeSpan.FromSeconds(10);
    }
}
