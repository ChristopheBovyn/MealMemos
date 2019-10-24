using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MealMemos.Droid
{
    public class MealAdapter : RecyclerView.Adapter
    {
        public List<string> dishes = new List<string>();
        public string Identifier = String.Empty;
        public MealAdapter(List<string> dishes,string identifier)
        {
            this.dishes = dishes;
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
            MealViewHolder viewHolder = new MealViewHolder(itemView,OnDishClick);
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
            this.Save()
;        }

        private void Save()
        {
           Preferences.Set(this.Identifier, JsonConvert.SerializeObject(this.dishes));
        }
    }
}
