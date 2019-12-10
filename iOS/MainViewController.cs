using System;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using UIImageExtension;
using UIKit;
using MealMemos.Extensions;
using System.Collections.Generic;
using GPS.iOS;
using Firebase.Auth;
using Foundation;
using MealMemos.ViewModels;
using MealMemos.iOS.Impl;

namespace MealMemos.iOS
{
    public partial class MainViewController : UIViewController
    {
        private TableViewSource viewSource;
        private MealViewModel mealViewModel;
        private readonly User user = Auth.DefaultInstance.CurrentUser;

        public MainViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.registerServices();
            this.mealViewModel = new MealViewModel(user.Uid, DateTime.Today);
            this.SetLayouts();
            this.SetViewsAction();
            this.SetTabBarItems();
            this.viewSource = new TableViewSource(new List<string>());
            this.InitData();
            this.viewSource.OnRemovedDish += OnRemovedDish;
        }

        private void OnRemovedDish(int index)
        {
            this.mealViewModel.RemoveDishAt(index, this.applicationTabBar.SelectedItem.Title);
            this.mealViewModel.SaveDishes(this.applicationTabBar.SelectedItem.Title);
            this.SetViewSourceData();
        }

        private void SetLayouts()
        {
            this.currentDateBtn.SetTitle(this.mealViewModel.ApiFormattedDateTime, UIControlState.Normal); ;
            this.mealTableView.TranslatesAutoresizingMaskIntoConstraints = false;
            this.mealTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            this.mealTableView.SetEditing(true, true);
            this.applicationTabBar.BackgroundColor = UIColor.FromName("applicationColor");
            this.addDishBtn.Layer.CornerRadius = this.addDishBtn.Frame.Width / 2;
        }

        private void SetViewsAction()
        {
            this.applicationTabBar.ItemSelected += SwitchMeal;
            this.previousBtn.TouchUpInside += previousAction;
            this.nextBtn.TouchUpInside += nextAction;
            this.currentDateBtn.TouchUpInside += OpenDatePicker;
            this.addDishBtn.TouchUpInside += AddDishAction;
            this.mealViewModel.OnMealDocumentChanged += OnMealDocumentChanged;
        }

        private void OnMealDocumentChanged()
        {
            this.SetViewSourceData();
        }

        private void InitData()
        {
            this.datepicker.Mode = UIDatePickerMode.Date;
            this.datepicker.SetDate((NSDate)this.mealViewModel.Date, false);
            this.mealTableView.Source = this.viewSource;
            this.mealViewModel.LoadDishesAsync().SafeFireAndForget();
        }

        private void SwitchMeal(object sender, UITabBarItemEventArgs e)
        {
            this.mealTableView.Source = this.viewSource;
            this.resetTableView();
        }

        private void AddDishAction(object sender, EventArgs e)
        {
            this.OpenPopup().SafeFireAndForget();
        }

        #region actions

        private void previousAction(object sender, EventArgs e)
        {
            this.mealViewModel.Date = this.mealViewModel.Date.AddDays(-1);
            this.UpdateDate();
        }

        private void nextAction(object sender, EventArgs e)
        {
            this.mealViewModel.Date = this.mealViewModel.Date.AddDays(1);
            this.UpdateDate();

        }

        private void UpdateDate()
        {
            this.currentDateBtn.SetTitle(this.mealViewModel.ApiFormattedDateTime, UIControlState.Normal);
            this.InitData();
            this.CloseDatePicker();
            this.resetTableView();
        }

        private void CloseDatePicker()
        {
            this.datepicker.Hidden = true;
            this.verticalSpace.Constant = 0;
        }

        private void OpenDatePicker(object sender, EventArgs e)
        {
            if (this.datepicker.Hidden)
            {
                this.datepicker.Hidden = false;
                this.verticalSpace.Constant = 150;
            }
            else
            {
                if (this.mealViewModel.Date != (DateTime)this.datepicker.Date)
                {
                    this.mealViewModel.Date = (DateTime)this.datepicker.Date;
                    this.currentDateBtn.SetTitle(this.mealViewModel.ApiFormattedDateTime, UIControlState.Normal);
                    this.InitData();
                }
                this.CloseDatePicker();
            }
        }

        #endregion actions

        private void resetTableView()
        {
            this.SetViewSourceData();
            this.mealTableView.ReloadData();
        }

        private void SetTabBarItems()
        {
            UIImage breakfast = UIImage.FromBundle("breakfast");
            UIImage diner = UIImage.FromBundle("diner");
            UIImage souper = UIImage.FromBundle("souper");
            UIImage collation = UIImage.FromBundle("collation");
            this.applicationTabBar.SelectedImageTintColor = UIColor.White;
            this.applicationTabBar.SelectedItem = this.applicationTabBar.Items[0];

            this.applicationTabBar.Items[0].Image = breakfast.ResizeImage(0.25f);
            this.applicationTabBar.Items[0].Title = "Breakfast";

            this.applicationTabBar.Items[1].Image = diner.ResizeImage(0.25f);
            this.applicationTabBar.Items[1].Title = "Diner";

            this.applicationTabBar.Items[2].Image = souper.ResizeImage(0.25f);
            this.applicationTabBar.Items[2].Title = "Souper";

            this.applicationTabBar.Items[3].Image = collation.ResizeImage(0.25f);
            this.applicationTabBar.Items[3].Title = "Collation";

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
            this.mealViewModel.AddDish(dishValue, this.applicationTabBar.SelectedItem.Title);
            this.mealViewModel.SaveDishes(this.applicationTabBar.SelectedItem.Title);
            this.SetViewSourceData();
        }

        private void SetViewSourceData()
        {
            this.viewSource.Dishes =
                this.mealViewModel.MealDocument.GetMeal(this.applicationTabBar.SelectedItem.Title);
            this.mealTableView.ReloadData();
        }

        private void registerServices()
        {
            SimpleIoc.Default.Register<IMealPopup>(() => { return new IosMealPopup(this); });
            SimpleIoc.Default.Register<IMealAPI>(() => { return new IOSMealAPI(); });
        }
    }
}