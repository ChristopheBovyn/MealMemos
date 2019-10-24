using System;
using Foundation;
using UIKit;

namespace MealMemos.iOS
{
    public partial class CustomViewCellController : UITableViewCell
	{
        public static string Identifier = "CustomViewCell";
        public string Title;
        //iden
        public CustomViewCellController (IntPtr handle) : base (handle)
		{
		}

        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {

        }

        public void Configure(string value)
        {
            MealTitleLabel.Text = value;
            DeleteMealBtn.ImageView.Image = UIImage.FromBundle("delete");
            DeleteMealBtn.Hidden = true;
            DeleteMealBtn.TouchUpInside += DeleteMealBtn_TouchUpInside;
        }

        private void DeleteMealBtn_TouchUpInside(object sender, EventArgs e)
        {
            
        }

        public void ShowDeleteBtn()
        {
            this.DeleteMealBtn.Hidden = !this.DeleteMealBtn.Hidden;
        }
    }
}
