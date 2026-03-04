using System;
using System.Threading;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Client;
using LaunchDarkly.Sdk.Client.Interfaces;

namespace DotNetConsoleApp
{
    public class Program
    {
        // Set mobileKey to your LaunchDarkly mobile key.
        const string mobileKey = "";

        // Set flagKey to the feature flag key you want to evaluate.
        const string flagKey = "sample-feature";

        static void Main(string[] args)
        {
            var resolvedMobileKey = GetMobileKey();
            var resolvedFlagKey = GetFlagKey();
            var isCi = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"));

            var client = LdClient.Init(
                Configuration.Default(resolvedMobileKey, ConfigurationBuilder.AutoEnvAttributes.Enabled),
                MakeContext(),
                TimeSpan.FromSeconds(10)
            );

            if (client.Initialized)
            {
                ShowMessage("SDK successfully initialized!");
            }
            else
            {
                ShowMessage("SDK failed to initialize. Please check your internet connection and SDK credential for any typo.");
                Environment.Exit(1);
            }

            var flagValue = client.BoolVariation(resolvedFlagKey, false);
            ShowMessage(string.Format("The {0} feature flag evaluates to {1}.", resolvedFlagKey, flagValue.ToString().ToLowerInvariant()));

            if (flagValue)
            {
                ShowAsciiArt();
            }

            if (!isCi)
            {
                client.FlagTracker.FlagValueChanged += (sender, eventArgs) =>
                {
                    if (eventArgs.Key == resolvedFlagKey)
                    {
                        flagValue = client.BoolVariation(resolvedFlagKey, false);
                        ShowMessage(string.Format("The {0} feature flag evaluates to {1}.", resolvedFlagKey, flagValue.ToString().ToLowerInvariant()));

                        if (flagValue)
                        {
                            ShowAsciiArt();
                        }
                    }
                };

                Thread.Sleep(Timeout.Infinite);
            }

            // Here we ensure that the SDK shuts down cleanly and has a chance to deliver analytics
            // events to LaunchDarkly before the program exits. If analytics events are not delivered,
            // the context properties and flag usage statistics will not appear on your dashboard. In
            // a normal long-running application, the SDK would continue running and events would be
            // delivered automatically in the background.
            client.Dispose();
        }

        static string GetMobileKey()
        {
            if (!string.IsNullOrEmpty(mobileKey))
            {
                return mobileKey;
            }

            var envKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_MOBILE_KEY");
            if (!string.IsNullOrEmpty(envKey))
            {
                return envKey;
            }

            ShowMessage("LaunchDarkly mobile key is required: set the mobileKey variable in Program.cs, or the LAUNCHDARKLY_MOBILE_KEY environment variable and try again.");
            Environment.Exit(1);
            return ""; // unreachable
        }

        static string GetFlagKey()
        {
            var envKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_FLAG_KEY");
            if (!string.IsNullOrEmpty(envKey))
            {
                return envKey;
            }

            if (!string.IsNullOrEmpty(flagKey))
            {
                return flagKey;
            }

            ShowMessage("LaunchDarkly flag key is required: set the flagKey variable in Program.cs, or the LAUNCHDARKLY_FLAG_KEY environment variable and try again.");
            Environment.Exit(1);
            return ""; // unreachable
        }

        // Set up the evaluation context. This context should appear on your
        // LaunchDarkly contexts dashboard soon after you run the demo.
        static Context MakeContext() =>
            Context.Builder("example-user-key")
                .Name("Sandy")
                .Build();

        static void ShowMessage(string s)
        {
            Console.WriteLine("*** " + s);
            Console.WriteLine();
        }

        static void ShowAsciiArt()
        {
            Console.WriteLine("        \u2588\u2588       ");
            Console.WriteLine("          \u2588\u2588     ");
            Console.WriteLine("      \u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588   ");
            Console.WriteLine("         \u2588\u2588\u2588\u2588\u2588\u2588\u2588 ");
            Console.WriteLine("\u2588\u2588 LAUNCHDARKLY \u2588");
            Console.WriteLine("         \u2588\u2588\u2588\u2588\u2588\u2588\u2588 ");
            Console.WriteLine("      \u2588\u2588\u2588\u2588\u2588\u2588\u2588\u2588   ");
            Console.WriteLine("          \u2588\u2588     ");
            Console.WriteLine("        \u2588\u2588       ");
            Console.WriteLine();
        }
    }
}
