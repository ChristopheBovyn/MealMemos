using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MealMemos.Droid
{
    public class InformationViewHolder : RecyclerView.ViewHolder
    {
        public TextView InformationTextView;

        public InformationViewHolder(View itemView) : base(itemView)
        {
            this.InformationTextView = itemView.FindViewById<TextView>(Resource.Id.informationTextView);
        }
    }
}
