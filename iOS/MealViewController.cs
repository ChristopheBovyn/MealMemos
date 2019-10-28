using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Extensions;
using MealMemos.Interfaces;
using Plugin.CloudFirestore;
using UIKit;

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
            this.viewSource = new TableViewSource(new List<string>());
            viewSource.Identifier = MealTitleText;
            this.LoadDishesAsync().SafeFireAndForget();
            this.TableView.Source = this.viewSource;
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            this.TableView.SetEditing(true, true);
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
            this.viewSource.Save();
        }

        private async Task LoadDishesAsync()
        {
            List<string> dishes = new List<string>();
            try
            {
                var document = await CrossCloudFirestore.Current
                           .Instance
                           .GetCollection("meals")
                           .GetDocument(this.MealTitleText)
                           .GetDocumentAsync();
                foreach (string dish in document.Data.Values)
                {
                    dishes.Add(dish);
                    this.viewSource.AddElement(dish);
                }
                this.TableView.ReloadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
