using System;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using Xamarin.Essentials;
using MealMemos.Extensions;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace MealMemos.Droid
{
    public class MealFragment : Android.Support.V4.App.Fragment
    {
        private const string PageTitle = "page_title";
        private TextView MealTitle;
        private RecyclerView RecyclerView;
        private List<string> MealList = new List<string>();

        private MealAdapter MealAdapter;

        public static MealFragment NewInstance(string pageTitle)
        {
            MealFragment mealFragment = new MealFragment();
            Bundle args = new Bundle();
            args.PutString(PageTitle, pageTitle);
            mealFragment.Arguments = args;
            return mealFragment;
        }

        //TODO : override OnCreate

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            string mealTitle = Arguments.GetString(PageTitle, "");
            View view = inflater.Inflate(Resource.Layout.meal, container, false);
            this.RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.meal_details);
            this.RecyclerView.SetLayoutManager(new LinearLayoutManager(Application.Context));
            this.MealAdapter = new MealAdapter(this.MealList);
            this.RecyclerView.SetAdapter(MealAdapter);

            this.MealTitle = (TextView)view.FindViewById(Resource.Id.meal_title);
            this.MealTitle.Text = mealTitle;

            var addButton = view.FindViewById<FloatingActionButton>(Resource.Id.add_aliment_dish);
            addButton.Click += this.AddButtonClick;
            return view;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            this.OpenPopup().SafeFireAndForget();
        }

        private async Task OpenPopup()
        {
            var result = await SimpleIoc.Default.GetInstance<IMealPopup>().OpenPopupWithResult();
            if (!result.IsNullOrEmpty())
            {
                this.SetMeal(result);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("MealFragment : AddButtonClick() : result is null");
            }
        }

        private void SetMeal(string meal)
        {
            this.MealList.Add(meal);
            this.MealAdapter.NotifyDataSetChanged();
        }

        private void RegisterMeal(string key,string value)
        {
              Preferences.Set(key, value);
        }
    }
}
