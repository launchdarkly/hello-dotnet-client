### LaunchDarkly Sample Xamarin Application
We've built 2 applications that demonstrates how LaunchDarkly's SDK works with both iOS and Android using the Xamarin platform.
Below, you'll find the basic build procedure, but for more comprehensive instructions, you can visit your [Quickstart page](https://app.launchdarkly.com/quickstart#/).
##### Build instructions

1. Make sure you have [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/) installed along with [Xcode](https://itunes.apple.com/us/app/xcode/id497799835?ls=1&mt=12).
2. Make sure you're in this directory.
3. Open `LaunchDarkly.Xamarin.sln` in Visual Studio for Mac.
4. Copy the mobile key from your account settings page and the feature flag key(s) from your LaunchDarkly dashboard into the appropriate fields in your source code. For the iOS project, this is at the top of the `MainCollectionViewController` class inside `MainCollectionViewController.cs.` For the Android project, this is at the top of the `MainActivity` class inside `MainActivity.cs` class. Note that the mobile apps require the LaunchDarkly mobile key to be set in order to run.
5. Select the platform you want to run on in Visual Studio's targeted device selector and press run.
