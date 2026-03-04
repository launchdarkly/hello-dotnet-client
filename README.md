# LaunchDarkly sample .NET client-side application

We've built simple demo applications that demonstrate how the LaunchDarkly client-side .NET SDK works. There are two demos:

- **Console app** (`DotNetConsoleApp`): A .NET console application.
- **MAUI app** (`MauiApp`): A .NET MAUI application targeting Android and iOS.

Below, you'll find the build procedures. For more comprehensive instructions, you can visit your [Quickstart page](https://app.launchdarkly.com/quickstart#/) or
the [client-side .NET SDK reference guide](https://docs.launchdarkly.com/sdk/client-side/dotnet).

These demos require .NET 8.0 or higher.

## Console app

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

## MAUI app (Android & iOS)

The MAUI app demonstrates LaunchDarkly in a mobile context using .NET MAUI, targeting both Android and iOS from a single project.

1. Install the MAUI workload if you haven't already:
    ```bash
    dotnet workload install maui
    ```

2. Set the value of the `mobileKey` variable in `MauiApp/MainPage.xaml.cs` to your mobile key:
    ```csharp
    const string mobileKey = "my-mobile-key";
    ```

3. Build and run:
    - **Android** (deploy to a connected device or emulator):
      ```bash
      dotnet build MauiApp -f net8.0-android -t:Run
      ```
    - **iOS** (requires macOS with Xcode; deploy to a simulator):
      ```bash
      dotnet build MauiApp -f net8.0-ios -t:Run
      ```

The app displays the current value of the feature flag. The background color changes from dark (#373841) to green (#00844B) when the flag evaluates to true. The app reacts to flag changes in real time.
