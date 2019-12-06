using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Widget;
using Android.Support.V7.App;
using System;
using MealMemos.ViewModels;
using Firebase.Auth;
using AsyncAwaitBestPractices;

namespace MealMemos.Droid
{
    [Activity(Label = "MealMemos")]
    public class MainActivity : AppCompatActivity
    {
        private BottomNavigationView BottomNavigationView;
        private TextView currentDate;
        private MealViewModel MealViewModel;
        private MealFragment[] Fragments = new MealFragment[4];
        private MealFragment activeFragment;
        private FirebaseUser user = CreateAccountActivity.FirebaseAuth.CurrentUser;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "MealMemos";
            this.MealViewModel = new MealViewModel(user.Uid, DateTime.Today);
            this.BottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.activity_main_bottom_navigation);
            this.BottomNavigationView.NavigationItemSelected += OnItemSelected;
            this.LoadFragments();
            this.SetBtnData();
            this.UpdateDate();
        }

        private void LoadFragments()
        {
            this.activeFragment = this.Fragments[0] = MealFragment.NewInstance(this.BottomNavigationView.Menu.GetItem(0).TitleFormatted.ToString(), MealViewModel);
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, this.activeFragment).Commit();
            for (var i = 1; i < Fragments.Length; i++)
            {
                this.Fragments[i] = MealFragment.NewInstance(this.BottomNavigationView.Menu.GetItem(i).TitleFormatted.ToString(), MealViewModel);
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.content_frame, this.Fragments[i]).Hide(this.Fragments[i]).Commit();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetBtnData()
        {
            var previousDayBtn = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.previous_btn);
            previousDayBtn.Click += previousDay;
            var nextDayBtn = FindViewById<Android.Support.V7.Widget.AppCompatImageButton>(Resource.Id.next_btn);
            nextDayBtn.Click += nextDay;
            this.currentDate = FindViewById<Button>(Resource.Id.dateSelectorBtn);
            this.currentDate.Click += OpenDatePicker;
            this.currentDate.Text = this.MealViewModel.Date.ToShortDateString();
        }

        void OpenDatePicker(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance((DateTime time) =>
            {
                this.MealViewModel.Date = time;
                this.UpdateDate();
            }, this.MealViewModel.Date);
            frag.Show(FragmentManager, DatePickerFragment.TAG);

        }

        private void previousDay(object sender, EventArgs e)
        {
            this.MealViewModel.Date = this.MealViewModel.Date.AddDays(-1);
            UpdateDate();
        }

        private void nextDay(object sender, EventArgs e)
        {
            this.MealViewModel.Date  = this.MealViewModel.Date.AddDays(1);
            UpdateDate();
        }

        private void UpdateDate()
        {
            currentDate.Text = this.MealViewModel.Date.ToShortDateString();
            this.MealViewModel.LoadDishesAsync().SafeFireAndForget();
        }

        private void OnItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            this.LoadFragment(e.Item.ItemId);
        }

        private void LoadFragment(int id)
        {
            switch (id)
            {
                case Resource.Id.menu_breakfast:
                    SupportFragmentManager.BeginTransaction().Hide(activeFragment).Show(this.Fragments[0]).Commit();
                    activeFragment = this.Fragments[0];
                    break;
                case Resource.Id.menu_diner:
                    SupportFragmentManager.BeginTransaction().Hide(activeFragment).Show(this.Fragments[1]).Commit();
                    activeFragment = this.Fragments[1];
                    break;
                case Resource.Id.menu_souper:
                    SupportFragmentManager.BeginTransaction().Hide(activeFragment).Show(this.Fragments[2]).Commit();
                    activeFragment = this.Fragments[2];
                    break;
                case Resource.Id.menu_collation:
                    SupportFragmentManager.BeginTransaction().Hide(activeFragment).Show(this.Fragments[3]).Commit();
                    activeFragment = this.Fragments[3];
                    break;
            }
        }
    }
}

