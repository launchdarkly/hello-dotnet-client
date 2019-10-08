## LaunchDarkly Sample Xamarin Application

To demonstrate usage of the LaunchDarkly Xamarin SDK, we've built two applications that both do the same thing: one for Android and one for iOS. The only differences between them are in the platform-specific application startup code and the user interface components. The user interface could be generalized between them using Xamarin Forms, but in this case the UI is so simple (just a single text field) that it has little effect on the demo code.

### Build instructions

1. Make sure you have [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) installed. If you will be running the iOS app, you must also have [Xcode](https://itunes.apple.com/us/app/xcode/id497799835?ls=1&mt=12).
2. Make sure you're in this directory.
3. Open `LaunchDarkly.HelloXamarin.sln` in Visual Studio for Mac.
4. Open `LaunchDarklyParameters.cs` in the `Shared` project. Set `MobileKey` and `FlagKey` to the mobile key for your LaunchDarkly environment and the key of a boolean feature flag in your environment. If you want to test feature flag targeting for different users, you can also change `UserKey` or add more properties in `DefaultUser`.
5. Run the `AndroidApp` or `iOSApp` project in Visual Studio for Mac.
