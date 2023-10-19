using System;
using LaunchDarkly.Hello;
using LaunchDarkly.Sdk.Client;
using ConfigurationBuilder = LaunchDarkly.Sdk.Client.ConfigurationBuilder;

namespace DotNetConsoleApp
{
    // This is the .NET Core console version of the LaunchDarkly client-side .NET SDK demo. The
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

            var client = LdClient.Init(
                Configuration.Default(DemoParameters.MobileKey, ConfigurationBuilder.AutoEnvAttributes.Enabled),
                DemoParameters.MakeDemoContext(),
                DemoParameters.SdkTimeout
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
            // the context properties and flag usage statistics will not appear on your dashboard. In
            // a normal long-running application, the SDK would continue running and events would be
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
