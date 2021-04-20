using Foundation;
using UIKit;

namespace LaunchDarkly.Hello
{
    // This is the iOS version of the LaunchDarkly Xamarin SDK demo. The non-platform-specific
    // classes DemoMessages and DemoParameters are defined in ../Shared.

    // This file is a minimal implementation of UIApplicationDelegate which in this example
    // has no special functionality. The logic for using the LaunchDarkly SDK is in
    // MainCollectionViewController.cs.

    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions) => true;

        public override void OnResignActivation(UIApplication application) { }

        public override void OnActivated(UIApplication application) { }

        public override void WillTerminate(UIApplication application) { }
    }
}

