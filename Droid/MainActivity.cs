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
using MealMemos.Droid.Utils;
using Android.Support.V7.App;
using System;

namespace MealMemos.Droid
{
    [Activity(Label = "MealMemos", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity
    {
        private BottomNavigationView BottomNavigationView;
        public static FirebaseApp firebaseApp;
        public static DateTime mealDay = DateTime.Today;
        private TextView current_date;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            firebaseApp = this.SetFirebaseApp();
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "MealMemos";
            this.setDateBtn();
            this.BottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.activity_main_bottom_navigation);
            this.BottomNavigationView.NavigationItemSelected += OnItemSelected;
            this.LoadFirstItemMenu();
            this.registerServices();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void LoadFirstItemMenu()
        {
            SupportFragmentManager.BeginTransaction()
                 .Replace(Resource.Id.content_frame, MealFragment.NewInstance(this.BottomNavigationView.Menu.GetItem(0).TitleFormatted.ToString())).Commit();
        }

        private void setDateBtn()
        {
            var previousDayBtn = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.previous_btn);
            previousDayBtn.Click += previousDay;
            var nextDayBtn = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.next_btn);
            nextDayBtn.Click += nextDay;
            this.current_date = FindViewById<Button>(Resource.Id.dateSelectorBtn);
            this.current_date.Click += OpenDatePicker;
            this.current_date.Text = mealDay.Date.ToShortDateString();
        }

        void OpenDatePicker(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                this.current_date.Text = time.ToShortDateString();
                mealDay = time;
                this.LoadFragment(this.BottomNavigationView.SelectedItemId);
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
            
        }

        private void previousDay(object sender, EventArgs e)
        {
            mealDay = mealDay.AddDays(-1);
            current_date.Text = mealDay.ToShortDateString();
            this.LoadFragment(this.BottomNavigationView.SelectedItemId);
        }

        private void nextDay(object sender, EventArgs e)
        {
            mealDay = mealDay.AddDays(1);
            current_date.Text = mealDay.ToShortDateString();
            this.LoadFragment(this.BottomNavigationView.SelectedItemId);
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
                .SetProjectId(FirebaseUtils.projet_id)
                .SetApplicationId(FirebaseUtils.application_id)
                .SetApiKey(FirebaseUtils.api_key)
                .SetDatabaseUrl(FirebaseUtils.database_url)
                .SetStorageBucket(FirebaseUtils.storage_bucket)
                .Build();

            return FirebaseApp.InitializeApp(this, options);
        }
    }
}

