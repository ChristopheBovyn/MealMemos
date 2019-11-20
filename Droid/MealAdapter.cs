using System;
using System.Collections.Generic;
using Android.Gms.Tasks;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util;
using MealMemos.Models;
namespace MealMemos.Droid
{
    public class MealAdapter : RecyclerView.Adapter, IOnSuccessListener
    {
        private string dateString = MainActivity.mealDay.Date.ToString("yyyy-MM-dd");
        private const string userKey = "user";
        private const string dateKey = "date";
        private string documentId = String.Empty;
        private FirebaseUser user = CreateAccountActivity.FirebaseAuth.CurrentUser;
        private MealDocument mealDocument = new MealDocument();
        public List<string> dishes = new List<string>();
        public string Identifier = String.Empty;
        public MealAdapter(List<string> dishes, string identifier)
        {
            this.dishes = dishes;
            this.LoadDishes();
            this.Identifier = identifier;
        }

        public override int ItemCount => this.dishes.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MealViewHolder viewHolder = holder as MealViewHolder;
            viewHolder.MealTitleTextView.Text = this.dishes[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_dish, parent, false);
            MealViewHolder viewHolder = new MealViewHolder(itemView, OnDishClick);
            return viewHolder;
        }

        private void OnDishClick(int position)
        {
            this.dishes.Remove(this.dishes[position]);
            this.NotifyDataSetChanged();
            this.Save();
        }

        public void AddDish(string dish)
        {
            this.dishes.Add(dish);
            this.NotifyDataSetChanged();
            this.Save();
        }

        private void Save()
        {
            HashMap content = new HashMap();
            for (int i = 0; i < this.dishes.Count; i++)
            {
                content.Put(""+(i + 1), this.dishes[i]);
            }
            HashMap meal = new HashMap();
            meal.Put(this.Identifier, content);
            if(this.documentId == String.Empty)
            {
                DocumentReference document = CreateAccountActivity.FirestoreDb
                                                             .Collection("meals")
                                                             .Document();

                meal.Put(userKey, user.Uid);
                meal.Put(dateKey,dateString);
                document.Set(meal);
            }
            else
            {
                DocumentReference document = CreateAccountActivity.FirestoreDb
                                                             .Collection("meals")
                                                             .Document(this.documentId);
                document.Update(this.Identifier, content);
            }
           
        }


        private void LoadDishes()
        {

            CreateAccountActivity.FirestoreDb.Collection("meals").WhereEqualTo(userKey, this.user.Uid)
                                   .WhereEqualTo(dateKey, dateString)
                                   .Get()
                                   .AddOnSuccessListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            var meals = new List<string>
            {
                "Breakfast",
                "Diner",
                "Souper",
                "Collation"
            };
            if (!snapshot.IsEmpty)
            {
                var document = snapshot.Documents[0];
                this.documentId = document.Id;
                string date = document?.Data[dateKey]?.ToString() ?? string.Empty;
                string userId = document?.Data[userKey]?.ToString() ?? string.Empty;
                this.mealDocument = new MealDocument(userId, date);

                foreach(var element in document.Data)
                {
                    var key = element.Key;
                    if (meals.Contains(key))
                    {
                        var listDish = new List<string>();
                        var content = document.Get(key);

                        var dictio = new JavaDictionary<string, string>(content.Handle,JniHandleOwnership.DoNotRegister);

                        foreach(KeyValuePair<string,string> item in dictio)
                        {
                            listDish.Add(item.Value);
                        }

                        if(key == this.Identifier)
                        {
                            this.dishes = listDish;
                            this.NotifyDataSetChanged();
                        }

                        this.mealDocument.SetMeal(listDish, key);
                    }
                }
            }
        }
    }
}
