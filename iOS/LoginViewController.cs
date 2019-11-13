// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using UIKit;
using Newtonsoft.Json;
using Firebase.Auth;

namespace MealMemos.iOS
{

    public partial class LoginViewController : UIViewController
    {
        private const string userKey = "user";

        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.emailTextView.BecomeFirstResponder();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (this.HasAlreadyCreateAccount())
            {
                var controller = this.Storyboard.InstantiateViewController("MainViewController") as MainViewController;
                this.NavigationController.PushViewController(controller, true);
            }
            this.loginBtn.TouchUpInside += LoginAsync;
            this.emailTextView.ResignFirstResponder();
            this.scrollView.Scrolled += (object sender, EventArgs e) =>
            {
                this.scrollView.EndEditing(true);
            };

            this.InitTextFields();
            this.resetBtn.TouchUpInside += Reset;   
        }

        private async void LoginAsync(object sender, EventArgs e)
        {
            if (this.IsValidLogin())
            {
                var success = await this.SignInFirebaseAsync(this.emailTextView.Text, this.passTextView.Text);

                if (success != null)
                {
                    this.StoreUserInfo(success);
                    var controller = this.Storyboard.InstantiateViewController("MainViewController") as MainViewController;
                    this.NavigationController.PushViewController(controller, true);
                }
                else
                {
                    Console.WriteLine("Mealmemos : LoginViewController : Erreur creation compte");
                }
            }
        }

        private void StoreUserInfo(User user)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            Preferences.Set(userKey, JsonConvert.SerializeObject(user,settings));
        }

        private async Task<User> SignInFirebaseAsync(string email, string password)
        {
            try
            {
                var result = await Auth.DefaultInstance.CreateUserAsync(email, password);
                if (result.User != null)
                {
                    return result.User;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return null;
        }

        private bool IsValidLogin()
        {
            if (this.IsValidEmail() &&
                this.passTextView.Text != String.Empty &&
                this.confirmTextView.Text != String.Empty &&
                this.confirmTextView.Text.Equals(this.passTextView.Text))
            {
                return true;
            }

            return false;
        }

        private void Reset(object sender, EventArgs e)
        {
            this.emailTextView.Text = String.Empty;
            this.passTextView.Text = String.Empty;
            this.confirmTextView.Text = String.Empty;
        }

        private bool IsValidEmail()
        {
            try
            {
                return Regex.IsMatch(this.emailTextView.Text,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private bool HasAlreadyCreateAccount()
        {
            var user = Preferences.Get(userKey, null);
            return user == null ? false : true;
        }

        private void InitTextFields()
        {
            this.emailTextView.ReturnKeyType = UIReturnKeyType.Next;
            this.passTextView.ReturnKeyType = UIReturnKeyType.Next;
            this.confirmTextView.ReturnKeyType = UIReturnKeyType.Done;

            this.emailTextView.Text = "test@gmail.com";
            this.passTextView.Text = "test123";
            this.confirmTextView.Text = "test123";

            this.emailTextView.ShouldReturn = (textfield) => {
                textfield.ResignFirstResponder();
                this.passTextView.BecomeFirstResponder();
                return true;
            };
            this.passTextView.ShouldReturn = (textfield) => {
                textfield.ResignFirstResponder();
                this.confirmTextView.BecomeFirstResponder();
                return true;
            };

            this.confirmTextView.ShouldReturn = (textfield) => {
                textfield.ResignFirstResponder();
                return true;
            };

            this.confirmTextView.EditingDidBegin += (object sender, EventArgs e) =>
            {
                this.emailTopConstraint.Constant -= 50;
            };

            this.confirmTextView.EditingDidEnd += (object sender, EventArgs e) =>
            {
                this.emailTopConstraint.Constant += 50;
            };
        }
    }
}
