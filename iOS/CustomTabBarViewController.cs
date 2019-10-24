using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using GPS.iOS;
using MealMemos.Interfaces;
using MealMemos.Models;
using UIKit;
using UIImageExtension;

namespace MealMemos.iOS
{
    public partial class CustomTabBarViewController : UITabBarController
    {
        private List<MealViewController> mealViewControllers = new List<MealViewController>();

        public CustomTabBarViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.registerServices();
            this.initMealViewControllers();
            this.TabBar.BackgroundColor = UIColor.Red;
            this.TabBar.Translucent = true;
            this.TabBar.TintColor = UIColor.White;
            this.ViewControllers = this.mealViewControllers.ToArray();
            this.setTabbarIcons();
        }

        private void initMealViewControllers()
        {
            MealViewController mealViewController = null;
            List<MealItem> listMeals = MealItem.GetDefaultMeals();
            for (int i = 0; i < listMeals.Count; i++)
            {
                mealViewController = (MealViewController)Storyboard.InstantiateViewController("MealViewController");
                mealViewController.Title = listMeals[i].mealTitle;
                mealViewController.MealTitleText = listMeals[i].mealTitle;
                this.mealViewControllers.Add(mealViewController);
            }
        }

        private void registerServices()
        {
            SimpleIoc.Default.Register<IMealPopup>(() => { return new IosMealPopup(this); });
        }


        private void setTabbarIcons()
        {
            UIImage breakfast = UIImage.FromBundle("breakfast");
            UIImage diner = UIImage.FromBundle("diner");
            UIImage souper = UIImage.FromBundle("souper");
            UIImage collation = UIImage.FromBundle("collation");
            this.TabBar.Items[0].Image = breakfast.ResizeImage(0.25f);
            this.TabBar.Items[1].Image = diner.ResizeImage(0.25f);
            this.TabBar.Items[2].Image = souper.ResizeImage(0.25f);
            this.TabBar.Items[3].Image = collation.ResizeImage(0.25f);
        }

    }
}
