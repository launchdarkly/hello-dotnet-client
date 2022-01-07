using UIKit;

namespace LaunchDarkly.Hello
{
    // This is the iOS version of the LaunchDarkly Xamarin SDK demo. The non-platform-specific
    // classes DemoMessages and DemoParameters are defined in ../Shared.

    // This file takes care of overall initialization of the iOS app. The logic for using the
    // LaunchDarkly SDK is in MainCollectionViewController.cs.

    public class Application
    {
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}
