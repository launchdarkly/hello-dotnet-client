using System;
using Common.Logging;
using LaunchDarkly.Logging;
using LaunchDarkly.Xamarin;

namespace LaunchDarkly.Hello
{
    // This is the .NET Core console version of the LaunchDarkly Xamarin SDK demo. The
    // non-platform-specific classes DemoMessages and DemoParameters are defined in ../Shared.

    public class Program
    {
        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(DemoParameters.MobileKey))
            {
                ShowMessage(DemoMessages.MobileKeyNotSet);
                Environment.Exit(1);
            }

            // Set up logging to log SDK messages to the console. ConsoleAdapter is a simple
            // Common.Logging implementation that's provided by LaunchDarkly; Common.Logging has
            // an equivalent class, but not on every platform.
            LogManager.Adapter = new ConsoleAdapter(LogLevel.Info);
            // 

            LdClient client = LdClient.Init(
                DemoParameters.MobileKey,
                DemoParameters.MakeDemoUser(),
                DemoParameters.SDKTimeout
            );

            if (client.Initialized)
            {
                ShowMessage(DemoMessages.InitializationSucceeded);
            }
            else
            {
                ShowMessage(DemoMessages.InitializationFailed);
                Environment.Exit(1);
            }

            var flagValue = client.BoolVariation(DemoParameters.FeatureFlagKey, false);

            ShowMessage(string.Format(DemoMessages.FlagValueIs, DemoParameters.FeatureFlagKey, flagValue));

            // Here we ensure that the SDK shuts down cleanly and has a chance to deliver analytics
            // events to LaunchDarkly before the program exits. If analytics events are not delivered,
            // the user properties and flag usage statistics will not appear on your dashboard. In a
            // normal long-running application, the SDK would continue running and events would be
            // delivered automatically in the background.
            client.Dispose();
        }

        private static void ShowMessage(string s)
        {
            Console.WriteLine("*** " + s);
            Console.WriteLine();
        }
    }
}
