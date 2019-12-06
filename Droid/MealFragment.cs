using System;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using MealMemos.Extensions;
using AsyncAwaitBestPractices;
using Android.Support.V7.Widget;
using System.Threading.Tasks;
using MealMemos.ViewModels;

namespace MealMemos.Droid
{
    public class MealFragment : Android.Support.V4.App.Fragment
    {
        private const string PageTitleArgKey = "page_title";

        private MealViewModel mealViewModel;

        private TextView MealTitle;
        private RecyclerView RecyclerView;
        private MealAdapter MealAdapter;
        private FloatingActionButton addButton;

        protected string Identifier { get; private set; }

        public static MealFragment NewInstance(string pageTitle, MealViewModel mealViewModel)
        {
            MealFragment mealFragment = new MealFragment();
            Bundle args = new Bundle();
            mealFragment.mealViewModel = mealViewModel;
            args.PutString(PageTitleArgKey, pageTitle);
            mealFragment.Arguments = args;
            return mealFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.Identifier = Arguments.GetString(PageTitleArgKey, "");
            View view = inflater.Inflate(Resource.Layout.meal, container, false);

            this.RecyclerView = view.FindViewById<RecyclerView>(Resource.Id.meal_details);
            this.RecyclerView.SetLayoutManager(new LinearLayoutManager(Application.Context));
            this.MealAdapter = new MealAdapter(this.Identifier, this.mealViewModel.GetMealByName(this.Identifier));
            this.RecyclerView.SetAdapter(MealAdapter);
            this.MealTitle = (TextView)view.FindViewById(Resource.Id.meal_title);
            this.MealTitle.Text = this.Identifier;

            this.mealViewModel.OnMealDocumentChanged += this.OnMealDocumentChanged;
            this.addButton = view.FindViewById<FloatingActionButton>(Resource.Id.add_aliment_dish);
            this.addButton.Click += this.AddButtonClick;
            this.MealAdapter.OnDishRemoved += OnDishRemoved;
            return view;
        }

        private void OnMealDocumentChanged()
        {
            this.MealAdapter?.SetData(this.mealViewModel.GetMealByName(this.Identifier));
        }

        private void OnDishRemoved(int position)
        {
            this.mealViewModel.RemoveDishAt(position, this.Identifier);
            this.mealViewModel.SaveDishes(this.Identifier);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            if(this.addButton != null)
            {
                this.addButton.Click -= this.AddButtonClick;
            }
            if(this.mealViewModel != null)
            {
                this.mealViewModel.OnMealDocumentChanged -= this.OnMealDocumentChanged;
            }
            if(this.MealAdapter != null)
            {
                this.MealAdapter.OnDishRemoved -= this.OnDishRemoved;
            }
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
                this.mealViewModel.AddDish(result, this.Identifier);
                this.mealViewModel.SaveDishes(this.Identifier);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("MealFragment : AddButtonClick() : result is null");
            }
        }
    }
}
