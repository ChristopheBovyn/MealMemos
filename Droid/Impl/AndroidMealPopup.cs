using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using MealMemos.Interfaces;
using Plugin.CurrentActivity;

namespace MealMemos.Droid.Impl
{
    public class AndroidMealPopup : IMealPopup
    {
        private TaskCompletionSource<string> taskCompletionSource;

        public Task<string> OpenPopupWithResult()
        {
            taskCompletionSource = new TaskCompletionSource<string>();
            string value = "";
            LayoutInflater layout = LayoutInflater.From(CrossCurrentActivity.Current.Activity);
            View view = layout.Inflate(Resource.Layout.dialog_add_dish, null);
            var editext = view.FindViewById<EditText>(Resource.Id.editNewDish);

            AlertDialog.Builder builder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
            builder.SetView(view);
            builder.SetCancelable(true);

            builder.SetPositiveButton("Add dish", (sender, args) =>
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
