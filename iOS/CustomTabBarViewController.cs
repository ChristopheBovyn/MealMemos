// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using Foundation;
using GalaSoft.MvvmLight.Ioc;
using GPS.iOS;
using MealMemos.Interfaces;
using MealMemos.Models;
using UIKit;
using Xamarin.Essentials;

namespace MealMemos.iOS
{
	public partial class CustomTabBarViewController : UITabBarController
	{
        private List<MemberViewController> memberViewControllers = new List<MemberViewController>();

        public CustomTabBarViewController (IntPtr handle) : base (handle)
		{
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.registerServices();
            this.initMemberViewControllers();
            this.ViewControllers = this.memberViewControllers.ToArray();
        }

        private void initMemberViewControllers()
        {
            Team team = new Team();
            MemberViewController memberViewController = null;
            for (int i = 0; i < team.MemberCount; i++)
            {
                memberViewController = (MemberViewController)Storyboard.InstantiateViewController("MemberViewController");
                memberViewController.View.BackgroundColor = ColorConverters.FromHex(team[i].ColorTheme).ToPlatformColor();
                memberViewController.Title = "Member " + (i+1);
                memberViewController.SetFirstNameLabel(team[i].Firstname);
                this.memberViewControllers.Add(memberViewController);
            }
        }

        private void registerServices()
        {
            SimpleIoc.Default.Register<IMemberPopup>(() => { return new IosMemberPopup(this); });
        }
    }
}
