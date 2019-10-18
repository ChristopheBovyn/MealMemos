using System;
using System.Collections.Generic;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MealMemos.Droid
{
    public class MealAdapter : RecyclerView.Adapter
    {
        public List<string> ListMeal;
        public event EventHandler<int> ItemClick;
        public MealAdapter(List<string> list)
        {
            this.ListMeal = list;
        }

        public override int ItemCount => this.ListMeal.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MealViewHolder viewHolder = holder as MealViewHolder;
            viewHolder.MealTitleTextView.Text = this.ListMeal[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_dish, parent, false);
            MealViewHolder viewHolder = new MealViewHolder(itemView,OnDishClick);
            return viewHolder;
        }

        public void addInformation(string information)
        {
            this.ListMeal.Add(information);
            this.NotifyDataSetChanged();
        }

        private void OnDishClick(int position)
        {
            this.ListMeal.Remove(this.ListMeal[position]);
            this.NotifyDataSetChanged();
        }
    }
}
