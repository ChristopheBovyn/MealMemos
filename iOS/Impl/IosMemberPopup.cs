using System;
using System.Threading.Tasks;
using CoreGraphics;
using MealMemos.Interfaces;
using MealMemos.iOS;
using UIKit;

namespace GPS.iOS
{
    public class IosMemberPopup : IMemberPopup
    {
        public UIViewController ViewController { get; }
        private TaskCompletionSource<string> taskCompletionSource;
        private string informationText;

        public IosMemberPopup()
        {

        }
        public IosMemberPopup(UIViewController viewController)
        {
            ViewController = viewController;
        }

        public void OpenPopup()
        {
            UITextField uiText = null;
            var alertController = UIAlertController.Create("Add member information", "", UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("Add", UIAlertActionStyle.Default, action => AddInformationFromTextField(uiText)));
            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
            
            alertController.AddTextField((uiTextField) =>
            {
                uiTextField.Placeholder = "Enter your information";
                uiText = uiTextField;
            });
            this.ViewController.PresentViewController(alertController, true, null);
        }

        private void AddInformationFromTextField(UITextField uiText)
        {
            this.informationText = uiText.Text;
            taskCompletionSource.SetResult(uiText.Text);
        }

        public Task<string> OpenPopupWithResult()
        {
            taskCompletionSource = new TaskCompletionSource<string>();

            UITextField uiText = null;
            var alertController = UIAlertController.Create("Add member information", "", UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("Add", UIAlertActionStyle.Default, action => AddInformationFromTextField(uiText)));
            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            alertController.AddTextField((uiTextField) =>
            {
                uiTextField.Placeholder = "Enter your information";
                uiText = uiTextField;
                
            });
            this.ViewController.PresentViewController(alertController, true, null);

            return taskCompletionSource.Task;
        }
    }
}
