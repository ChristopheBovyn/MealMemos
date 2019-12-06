using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;

namespace MealMemos.Droid
{
    public class MealAdapter : RecyclerView.Adapter
    {
        public string Identifier;
        private List<string> dishes;
        public Action<int> OnDishRemoved { get; set; }

        public MealAdapter(string identifier, List<string> dishes)
        {
            this.Identifier = identifier;
            this.dishes = dishes;
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

        // On click on trash
        private void OnDishClick(int position)
        {
            //this.dishes.RemoveAt(position);
            this.OnDishRemoved?.Invoke(position);
            this.NotifyDataSetChanged();
        }

        public void SetData(List<string> list)
        {
            if(list != null)
            {
                this.dishes = list;
                this.NotifyDataSetChanged();
            }
        }
    }
}
