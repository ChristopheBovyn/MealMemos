using System;
using System.Net;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MealMemos.Droid
{
    public class MealViewHolder : RecyclerView.ViewHolder
    {
        public TextView MealTitleTextView;
        private ImageView delateImageView;
        private readonly Action<int> listener;

        public MealViewHolder(View itemView,Action<int> listener) : base(itemView)
        {
            this.MealTitleTextView = itemView.FindViewById<TextView>(Resource.Id.meal_title_textView);
            this.delateImageView = itemView.FindViewById<ImageView>(Resource.Id.delete_dish_icon);
            this.listener = listener;
            this.MealTitleTextView.Click += ItemView_Click;
            this.delateImageView.Click += DeleteImageView_Click;
        }

        private void ItemView_Click(object sender, EventArgs e)
        {
            
            if(this.delateImageView.Visibility == ViewStates.Visible)
            {
                this.delateImageView.Visibility = ViewStates.Invisible;
            }
            else
            {
                this.delateImageView.Visibility = ViewStates.Visible;
            }
        }

        private void DeleteImageView_Click(object sender, EventArgs e)
        {
            listener(LayoutPosition);
        }

    }
}
