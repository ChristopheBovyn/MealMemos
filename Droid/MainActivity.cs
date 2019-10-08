using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Android.Runtime;
using MealMemos.Models;

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
            var backgroundsColor = setColorList();
            TeamAdapter adapter = new TeamAdapter(SupportFragmentManager, team,backgroundsColor);
            viewPager.Adapter = adapter;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private List<string> setColorList()
        {
            List<string> colors = new List<string>();
            colors.Add(Color.Blue);
            colors.Add(Color.Gray);
            colors.Add(Color.Green);
            colors.Add(Color.Yellow);
            colors.Add(Color.Magenta);
            return colors;
        }
    }
}

