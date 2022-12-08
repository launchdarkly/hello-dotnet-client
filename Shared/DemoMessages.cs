
namespace LaunchDarkly.Hello
{
    // These strings are used by all three versions of the demo: XamarinAndroidApp,
    // XamarinIOsApp, and DotNetConsoleApp.

    public static class DemoMessages
    {
        public const string FlagValueIs = "Feature flag '{0}' is {1} for this context";

        public const string MobileKeyNotSet =
            "Please edit Shared/DemoParameters.cs to set Mobile to your LaunchDarkly mobile key first";

        public const string InitializationSucceeded = "SDK successfully initialized!";

        public const string InitializationFailed = "SDK failed to initialize";
    }
}
