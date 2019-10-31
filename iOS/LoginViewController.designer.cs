// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MealMemos.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UITextField confirmTextView { get; set; }

		[Outlet]
		UIKit.UITextField emailTextView { get; set; }

		[Outlet]
		UIKit.UIButton loginBtn { get; set; }

		[Outlet]
		UIKit.UITextField passTextView { get; set; }

		[Outlet]
		UIKit.UIButton resetBtn { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (loginBtn != null) {
				loginBtn.Dispose ();
				loginBtn = null;
			}

			if (resetBtn != null) {
				resetBtn.Dispose ();
				resetBtn = null;
			}

			if (confirmTextView != null) {
				confirmTextView.Dispose ();
				confirmTextView = null;
			}

			if (passTextView != null) {
				passTextView.Dispose ();
				passTextView = null;
			}

			if (emailTextView != null) {
				emailTextView.Dispose ();
				emailTextView = null;
			}
		}
	}
}
