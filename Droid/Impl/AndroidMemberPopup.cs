﻿using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using MealMemos.Interfaces;

namespace MealMemos.Droid.Impl
{
    public class AndroidMemberPopup : IMemberPopup
    {
        private Context context;
        private TaskCompletionSource<string> taskCompletionSource;

        public AndroidMemberPopup(Context context)
        {
            this.context = context;
        }

        public void OpenPopup()
        {

        }

        public Task<string> OpenPopupWithResult()
        {
            taskCompletionSource = new TaskCompletionSource<string>();

            string value = "";
            LayoutInflater layout = LayoutInflater.From(this.context);
            View view = layout.Inflate(Resource.Layout.memberPopup, null);
            var editext = view.FindViewById<EditText>(Resource.Id.editNewInfo);

            AlertDialog.Builder builder = new AlertDialog.Builder(this.context);
            builder.SetView(view);
            builder.SetCancelable(true);

            builder.SetPositiveButton("Add information", (sender, args) =>
            {
                value = editext.Text;
                taskCompletionSource.SetResult(value);
            });

            AlertDialog dialog = builder.Create();
            dialog.Show();

            return taskCompletionSource.Task;
        }
    }
}
