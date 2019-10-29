using System;
using System.Collections.Generic;
using Android.Gms.Tasks;
using Android.Support.V7.Widget;
using Android.Views;
using Firebase.Firestore;
using Java.Util;

namespace MealMemos.Droid
{
    public class MealAdapter : RecyclerView.Adapter,IOnSuccessListener
    {
        public List<string> dishes = new List<string>();
        public string Identifier = String.Empty;
        private string dateString = MainActivity.mealDay.Date.ToString("yyyy-MM-dd");
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
            HashMap map = new HashMap();
            for (int i = 0; i < this.dishes.Count; i++)
            {
                map.Put("dish" + (i + 1), this.dishes[i]);
            }
            FirebaseFirestore db = FirebaseFirestore.GetInstance(MainActivity.firebaseApp);
            Console.WriteLine("date : "+this.dateString);
            DocumentReference document = db.Collection("meals").Document("defaultUser").Collection(this.dateString).Document(this.Identifier);
            document.Set(map);
        }


        private void LoadDishes()
        {
            FirebaseFirestore db = FirebaseFirestore.GetInstance(MainActivity.firebaseApp);
            db.Collection("meals").Document("defaultUser").Collection(this.dateString).Get().AddOnSuccessListener(this);
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
            else
            {
                
            }
        }
    }
}
