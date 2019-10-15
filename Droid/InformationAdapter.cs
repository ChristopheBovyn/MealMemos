using System;
using System.Collections.Generic;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MealMemos.Droid
{
    public class InformationAdapter : RecyclerView.Adapter
    {
        public List<string> Informations;
        public InformationAdapter(List<string> infos)
        {
            this.Informations = infos;
        }

        public override int ItemCount => this.Informations.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            InformationViewHolder viewHolder = holder as InformationViewHolder;
            viewHolder.InformationTextView.Text = this.Informations[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.information, parent, false);
            InformationViewHolder viewHolder = new InformationViewHolder(itemView);

            return viewHolder;
        }

        public void addInformation(string information)
        {
            this.Informations.Add(information);
            this.NotifyDataSetChanged();
        }
    }
}
