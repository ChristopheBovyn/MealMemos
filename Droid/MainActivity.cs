using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Runtime;
using MealMemos.Models;
using Android.Support.Design.Widget;
using MealMemos.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Droid.Impl;
using Android.Widget;
using Firebase;

namespace MealMemos.Droid
{
    [Activity(Label = "MealMemos", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : FragmentActivity
    {
        private BottomNavigationView BottomNavigationView;
        public static FirebaseApp firebaseApp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            firebaseApp = this.SetFirebaseApp();
            SetContentView(Resource.Layout.Main);
            Team team = new Team();
            this.BottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.activity_main_bottom_navigation);
            this.BottomNavigationView.NavigationItemSelected += OnItemSelected;

            var contentFrame = FindViewById<FrameLayout>(Resource.Id.content_frame);
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, MealFragment.NewInstance(this.BottomNavigationView.Menu.GetItem(0).TitleFormatted.ToString())).Commit();
            this.registerServices();

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void registerServices()
        {
            SimpleIoc.Default.Register<IMealPopup>(() => { return new AndroidMealPopup(this); });
        }

        private void OnItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            this.LoadFragment(e.Item.ItemId);
        }

        private void LoadFragment(int id)
        {
            MealFragment MealFragment = null;
            switch (id)
            {
                case Resource.Id.menu_breakfast:
                    MealFragment = MealFragment.NewInstance(this.BottomNavigationView.Menu.GetItem(0).TitleFormatted.ToString());
                    break;
                case Resource.Id.menu_diner:
                    MealFragment = MealFragment.NewInstance(this.BottomNavigationView.Menu.GetItem(1).TitleFormatted.ToString());
                    break;
                case Resource.Id.menu_souper:
                    MealFragment = MealFragment.NewInstance(this.BottomNavigationView.Menu.GetItem(2).TitleFormatted.ToString());
                    break;
                case Resource.Id.menu_collation:
                    MealFragment = MealFragment.NewInstance(this.BottomNavigationView.Menu.GetItem(3).TitleFormatted.ToString());
                    break;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, MealFragment).Commit();
        }

        private FirebaseApp SetFirebaseApp()
        {
            var options = new FirebaseOptions.Builder()
                .SetProjectId("mealmemos-85014")
                .SetApplicationId("mealmemos-85014")
                .SetApiKey("AIzaSyAaDOE04aq2iqlWYsGOUZexHWLoLQj3fgU")
                .SetDatabaseUrl("https://mealmemos-85014.firebaseio.com")
                .SetStorageBucket("mealmemos-85014.appspot.com")
                .Build();

            return FirebaseApp.InitializeApp(this, options);
        }
    }
}

