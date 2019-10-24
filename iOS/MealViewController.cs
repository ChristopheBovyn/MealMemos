using System;
using UIKit;
using MealMemos.Extensions;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using AsyncAwaitBestPractices;
using Masonry;
using System.Collections.Generic;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace MealMemos.iOS
{
    public partial class MealViewController : UIViewController
    {
        public string MealTitleText;
        public int index;
        private TableViewSource viewSource;
        public MealViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (!this.MealTitleText.IsNullOrEmpty())
            {
                this.mealTitleLabel.Text = this.MealTitleText;
            }
            this.TableView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.addDishButton.Layer.CornerRadius = this.addDishButton.Frame.Width / 2;
            this.addDishButton.TouchUpInside += this.AddDishAction;
            this.viewSource = new TableViewSource(this.LoadDishes());
            this.TableView.Source = this.viewSource;
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            this.TableView.SetEditing(true, true);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            this.viewSource.Save(this.MealTitleText);
        }

        private void AddDishAction(object sender, EventArgs e)
        {
            this.OpenPopup().SafeFireAndForget();
        }

        private async Task OpenPopup()
        {
            var result = await SimpleIoc.Default.GetInstance<IMealPopup>().OpenPopupWithResult();
            if (!result.IsNullOrEmpty())
            {
                this.SetDish(result);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("MealViewController : OpenPopup() : result is null");
            }
        }

        private void SetDish(string dishValue)
        {
            this.viewSource.AddElement(dishValue);
            this.TableView.ReloadData();
            this.viewSource.Save(this.MealTitleText);
        }

        private List<string> LoadDishes()
        {
            List<string> dishes = new List<string>();
            var json = Preferences.Get(this.MealTitleText, null);
            if(json != null)
            {
                dishes = JsonConvert.DeserializeObject<List<string>>(json);
            }
            return dishes;
        }
    }
}
