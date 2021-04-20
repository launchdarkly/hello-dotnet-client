using System;
using LaunchDarkly.Client;

namespace LaunchDarkly.Hello
{
    // These values are used by all three versions of the demo: XamarinAndroidApp,
    // XamarinIOsApp, and XamarinConsoleApp.

    public class DemoParameters
    {
        // Set MobileKey to your LaunchDarkly mobile key.
        public const string MobileKey = "mob-f0c2ce9a-fdca-4190-b0da-8f6645a8330e";

        // Set FeatureFlagKey to the feature flag key you want to evaluate.
        public const string FeatureFlagKey = "my-boolean-flag";

        // Set up the user properties. This user should appear on your LaunchDarkly users dashboard
        // soon after you run the demo.
        public static User MakeDemoUser() =>
            User.Builder("example-user-key")
                .Name("Sandy")
                .Build();

        // How long the application will wait for the SDK to connect to LaunchDarkly
        public static TimeSpan SDKTimeout = TimeSpan.FromSeconds(10);
    }
}
