// This file has been autogenerated from a class added in the UI designer.

using System;
using UIKit;
using MealMemos.Models;
using System.Linq;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using GPS.iOS;

namespace MealMemos.iOS
{
    public partial class HomepageViewController : UIViewController, IUIPageViewControllerDataSource
    {
        private Team team;
        private int currentMealIndex = 0;
        private UIPageViewController pageViewController;
        private UIPageControl pageControl;

        public HomepageViewController(IntPtr handle) : base(handle)
        {
            this.team = new Team();            
        }

        public override void ViewDidLoad()
        {
            this.registerServices();
        }

        private void PageViewControllerDidFinishAnimating(object sender, UIPageViewFinishedAnimationEventArgs e)
        {
            if (e.Finished && e.Completed)
            {
                // Get children of pageViewController
                // Get the current displayed
                // Cast as MemberViewController
                // Get index
                this.pageControl.CurrentPage = (this.pageViewController.ViewControllers?.FirstOrDefault() as MealViewController).index;
            }
        }

        private MealViewController MealViewControllerAt(int index)
        {
            if (index >= team.MemberCount || team.MemberCount == 0)
            {
                return null;
            }
            var mealViewController = (MealViewController)Storyboard.InstantiateViewController("MealViewController");
            mealViewController.MealTitleText = this.team[index].Firstname;
            mealViewController.index = index;
            return mealViewController;
        }

        public UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var current = (MealViewController)referenceViewController;
            var index = current.index;
            if (index == 0)
            {
                return null;
            }
            index--;
            return this.MealViewControllerAt(index);
        }

        public UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var current = (MealViewController)referenceViewController;
            var index = current.index;
            if (index == team.MemberCount)
            {
                return null;
            }
            index++;
            return this.MealViewControllerAt(index);
        }

        private void registerServices()
        {
            SimpleIoc.Default.Register<IMealPopup>(() => { return new IosMealPopup(this.MealViewControllerAt(this.currentMealIndex)); });
        }
    }
}
