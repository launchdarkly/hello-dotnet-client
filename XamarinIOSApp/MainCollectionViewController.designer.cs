// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LaunchDarkly.Hello
{
    [Register ("MainCollectionViewController")]
    partial class MainCollectionViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView MessageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MessageView != null) {
                MessageView.Dispose ();
                MessageView = null;
            }
        }
    }
}