using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Android.Runtime;
using MealMemos.Models;
using Android.Support.Design.Widget;
using Android.Support.V7.App;

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
            var backgroundsColor = SetColorList();
            TeamAdapter adapter = new TeamAdapter(SupportFragmentManager, team,backgroundsColor);
            viewPager.Adapter = adapter;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private List<string> SetColorList()
        {
            List<string> colors = new List<string>
            {
                Color.Blue,
                Color.Gray,
                Color.Green,
                Color.Yellow,
                Color.Magenta
            };
            return colors;
        }
    }
}

