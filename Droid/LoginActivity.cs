
using System;
using MealMemos.Extensions;
using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Auth;

namespace MealMemos.Droid
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private EditText emailEdit;
        private EditText passwordEdit;
        private const string userKey = "user";
        private TextView authFailed;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            this.emailEdit = FindViewById<EditText>(Resource.Id.email_edit_text);
            this.passwordEdit = FindViewById<EditText>(Resource.Id.password_edit_text);
            this.passwordEdit.Text = "test1234";
            this.authFailed = FindViewById<TextView>(Resource.Id.auth_failure);
            if (CreateAccountActivity.FirebaseAuth?.CurrentUser?.Email != null)
                this.emailEdit.Text = CreateAccountActivity.FirebaseAuth?.CurrentUser?.Email;
            this.emailEdit.TextChanged += (sender, args) =>
            {
                this.authFailed.Visibility = Android.Views.ViewStates.Invisible;
            };
            this.passwordEdit.TextChanged += (sender, args) =>
            {
                this.authFailed.Visibility = Android.Views.ViewStates.Invisible;
            };
            var loginBtn = FindViewById<Button>(Resource.Id.login_btn);
            var resetBtn = FindViewById<Button>(Resource.Id.reset_btn);
            loginBtn.Click += LogUser;
            resetBtn.Click += ResetForm;
        }

        private void ResetForm(object sender, EventArgs e)
        {
            this.emailEdit.Text = this.passwordEdit.Text = String.Empty;
        }

        private async void LogUser(object sender, EventArgs e)
        {
            if (this.IsValidForm())
            {
                var email = this.emailEdit.Text;
                var password = this.passwordEdit.Text;
                try
                {
                    await CreateAccountActivity.FirebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
                    StartActivity(typeof(MainActivity));
                }
               catch(FirebaseAuthException ex)
                {
                    Console.WriteLine(ex);
                    this.authFailed.Visibility = Android.Views.ViewStates.Visible;
                }
            }
            else
            {
                this.authFailed.Visibility = Android.Views.ViewStates.Visible;
            }
        }

        private bool IsValidForm()
        {
            if (this.emailEdit.Text.isAValidEmail() && this.passwordEdit.Text != String.Empty)
                return true;
            return false;
        }
    }
}
