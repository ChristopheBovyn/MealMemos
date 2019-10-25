using System;
using System.Collections.Generic;
using Android.Gms.Tasks;
using Android.Support.V7.Widget;
using Android.Views;
using Firebase.Firestore;
using Java.Util;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MealMemos.Droid
{
    public class MealAdapter : RecyclerView.Adapter,IOnSuccessListener
    {
        public List<string> dishes = new List<string>();
        public string Identifier = String.Empty;
        public MealAdapter(List<string> dishes, string identifier)
        {
            this.dishes = dishes;
            this.LoadDishes();
            Console.WriteLine("init adapter");
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
            Preferences.Set(this.Identifier, JsonConvert.SerializeObject(this.dishes));
            HashMap map = new HashMap();
            for (int i = 0; i < this.dishes.Count; i++)
            {
                map.Put("dish" + (i + 1), this.dishes[i]);
            }
            FirebaseFirestore db = FirebaseFirestore.GetInstance(MainActivity.firebaseApp);
            DocumentReference document = db.Collection("meals").Document(this.Identifier);
            document.Set(map);
        }


        private void LoadDishes()
        {
            FirebaseFirestore db = FirebaseFirestore.GetInstance(MainActivity.firebaseApp);
            db.Collection("meals").Get().AddOnSuccessListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                foreach (DocumentSnapshot document in documents)
                {
                    if (document.Id == this.Identifier)
                    {
                        foreach (string dish in document.Data.Values)
                        {
                            this.dishes.Add(dish);
                        }
                        this.NotifyDataSetChanged();
                        break;
                    }
                }
            }
        }
    }
}
