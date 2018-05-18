using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Foundation;
using UIKit;

namespace LaunchDarkly.Xamarin.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            var props = new global::Common.Logging.Configuration.NameValueCollection
            {
                { "level", "Debug" }
            };
            LogManager.Adapter = new global::Common.Logging.Simple.DebugLoggerFactoryAdapter(props);

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
