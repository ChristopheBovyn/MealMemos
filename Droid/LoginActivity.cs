
using System;
using MealMemos.Extensions;
using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Auth;
using Android.Views;

namespace MealMemos.Droid
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private EditText emailEdit;
        private EditText passwordEdit;
        private TextView emailError;
        private TextView passwordError;
        private Button loginBtn;
        private Button resetBtn;
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
                this.authFailed.Visibility = ViewStates.Invisible;
            };
            this.passwordEdit.TextChanged += (sender, args) =>
            {
                this.authFailed.Visibility = ViewStates.Invisible;
            };
            this.EmailActions();
            this.PasswordActions();
            this.emailError = FindViewById<TextView>(Resource.Id.email_error);
            this.passwordError = FindViewById<TextView>(Resource.Id.password_error);
            this.loginBtn = FindViewById<Button>(Resource.Id.login_btn);
            this.resetBtn = FindViewById<Button>(Resource.Id.reset_btn);
            loginBtn.Click += LogUser;
            resetBtn.Click += ResetForm;
        }

        private void ResetForm(object sender, EventArgs e)
        {
            this.emailEdit.Text = this.passwordEdit.Text = String.Empty;
        }

        private async void LogUser(object sender, EventArgs e)
        {
            var creating = FindViewById<RelativeLayout>(Resource.Id.creating_account);
            creating.Visibility = ViewStates.Visible;
            if (this.IsValidForm())
            {
                var email = this.emailEdit.Text;
                var password = this.passwordEdit.Text;
                try
                {
                    await CreateAccountActivity.FirebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
                    creating.Visibility = ViewStates.Visible;
                    StartActivity(typeof(MainActivity));
                }
               catch(FirebaseAuthException ex)
                {
                    Console.WriteLine(ex);
                    this.authFailed.Visibility = ViewStates.Visible;
                }
            }
            else
            {
                this.authFailed.Visibility = ViewStates.Visible;
            }
        }

        private bool IsValidForm()
        {
            if (this.emailEdit.Text.IsAValidEmail() && this.passwordEdit.Text != String.Empty)
                return true;
            return false;
        }

        private void EmailActions()
        {
            this.emailEdit.AfterTextChanged += (sender, args) =>
            {
                if (!this.emailEdit.Text.IsAValidEmail())
                    this.emailError.Visibility = ViewStates.Visible;
                else
                    this.emailError.Visibility = ViewStates.Invisible;
                if (this.IsValidForm())
                {
                    this.loginBtn.Enabled = true;
                    this.loginBtn.SetBackgroundResource(Resource.Color.colorAccentDark);
                }
            };
        }

        private void PasswordActions()
        {
            this.passwordEdit.AfterTextChanged += (sender, args) =>
            {

                if (this.passwordEdit.Text.Length < 6)
                    this.passwordError.Visibility = ViewStates.Visible;
                else
                    this.passwordError.Visibility = ViewStates.Invisible;
                if (this.IsValidForm())
                {
                    this.loginBtn.Enabled = true;
                    this.loginBtn.SetBackgroundResource(Resource.Color.colorAccentDark);
                }
            };
        }
    }
}
