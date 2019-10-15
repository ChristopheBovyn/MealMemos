using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Android.Runtime;
using MealMemos.Models;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System;
using Android.Widget;
using MealMemos.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Droid.Impl;

namespace MealMemos.Droid
{
    [Activity(Label = "MealMemos", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : FragmentActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            Team team = new Team();
            TabsAdapter tabsAdapter = new TabsAdapter(SupportFragmentManager);
            var tabbar = FindViewById<TabLayout>(Resource.Id.bottomBar);
            tabbar.SetupWithViewPager(viewPager);
            viewPager.Adapter = tabsAdapter;
            viewPager.OffscreenPageLimit = 2;
            viewPager.PageSelected += (sender, args) =>
            {
                var fragment = tabsAdapter.InstantiateItem(viewPager, args.Position);
            };
            this.registerServices();
           
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void registerServices()
        {
            SimpleIoc.Default.Register<IMemberPopup>(() => { return new AndroidMemberPopup(this); });
        }
    }
}

