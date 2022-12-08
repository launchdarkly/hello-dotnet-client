using System;
using LaunchDarkly.Sdk;

namespace LaunchDarkly.Hello
{
    // These values are used by all three versions of the demo: XamarinAndroidApp,
    // XamarinIOsApp, and XamarinConsoleApp.

    public class DemoParameters
    {
        // Set MobileKey to your LaunchDarkly mobile key.
        public const string MobileKey = "";

        // Set FeatureFlagKey to the feature flag key you want to evaluate.
        public const string FeatureFlagKey = "my-boolean-flag";

        // Set up the evaluation context. This context should appear on your LaunchDarkly
        // contexts dashboard soon after you run the demo.
        public static Context MakeDemoContext() =>
            Context.Builder("example-user-key")
                .Name("Sandy")
                .Build();

        // How long the application will wait for the SDK to connect to LaunchDarkly
        public static TimeSpan SDKTimeout = TimeSpan.FromSeconds(10);
    }
}
