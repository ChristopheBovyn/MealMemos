using System.Threading.Tasks;
using MealMemos.Interfaces;
using UIKit;

namespace GPS.iOS
{
    public class IosMealPopup : IMealPopup
    {
        public UIViewController ViewController { get; }
        private TaskCompletionSource<string> taskCompletionSource;
        private string dishText;

        public IosMealPopup(UIViewController viewController)
        {
            ViewController = viewController;
        }

        public Task<string> OpenPopupWithResult()
        {
            taskCompletionSource = new TaskCompletionSource<string>();

            UITextField uiText = null;
            var alertController = UIAlertController.Create("Add aliment / dish", "", UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("Add", UIAlertActionStyle.Default, action => AddDishTextField(uiText)));
            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            alertController.AddTextField((uiTextField) =>
            {
                uiTextField.Placeholder = "Enter an aliment / dish";
                uiText = uiTextField;
                
            });
            this.ViewController.PresentViewController(alertController, true, null);

            return taskCompletionSource.Task;
        }

        private void AddDishTextField(UITextField uiText)
        {
            this.dishText = uiText.Text;
            taskCompletionSource.SetResult(uiText.Text);
        }
    }
}
