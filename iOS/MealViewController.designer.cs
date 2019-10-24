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
	[Register ("MealViewController")]
	partial class MealViewController
	{
		[Outlet]
		UIKit.UIButton addDishButton { get; set; }

		[Outlet]
		UIKit.UILabel mealTitleLabel { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (addDishButton != null) {
				addDishButton.Dispose ();
				addDishButton = null;
			}

			if (mealTitleLabel != null) {
				mealTitleLabel.Dispose ();
				mealTitleLabel = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
