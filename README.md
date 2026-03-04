# LaunchDarkly sample .NET client-side application

We've built a simple console application that demonstrates how the LaunchDarkly client-side .NET SDK works.

Below, you'll find the build procedure. For more comprehensive instructions, you can visit your [Quickstart page](https://app.launchdarkly.com/quickstart#/) or
the [client-side .NET SDK reference guide](https://docs.launchdarkly.com/sdk/client-side/dotnet).

This demo requires .NET 8.0 or higher.

## Build instructions

1. Set the value of the `mobileKey` variable in `DotNetConsoleApp/Program.cs` to your mobile key:
    ```csharp
    const string mobileKey = "my-mobile-key";
    ```
    Alternatively, set the `LAUNCHDARKLY_MOBILE_KEY` environment variable:
    ```bash
    export LAUNCHDARKLY_MOBILE_KEY="my-mobile-key"
    ```

2. If there is an existing boolean feature flag in your LaunchDarkly project that
   you want to evaluate, set `flagKey` to the flag key:
    ```csharp
    const string flagKey = "my-flag-key";
    ```
    Otherwise, `sample-feature` will be used by default.

3. On the command line, run:
    ```bash
    dotnet run --project DotNetConsoleApp
    ```
    You should receive the message:
    > "The sample-feature feature flag evaluates to false."

The application will run continuously and react to the flag changes in LaunchDarkly.
