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
	[Register ("MemberViewController")]
	partial class MemberViewController
	{
		[Outlet]
		UIKit.UILabel firstnameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (firstnameLabel != null) {
				firstnameLabel.Dispose ();
				firstnameLabel = null;
			}
		}
	}
}
