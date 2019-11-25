using System;
using MealMemos.Extensions;
using Android.App;
using Android.OS;
using Android.Widget;
using Firebase;
using MealMemos.Droid.Utils;
using Firebase.Firestore;
using Firebase.Auth;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using MealMemos.Droid.Impl;
using Plugin.CurrentActivity;
using MealMemos.Models;
using Android.Support.V4.Content;
using Android.Runtime;
using Android.Views;

namespace MealMemos.Droid
{
    [Activity(Label = "MealMemos", MainLauncher = true, Icon = "@mipmap/icon")]
    public class CreateAccountActivity : Activity
    {
        private EditText emailEdit;
        private EditText passwordEdit;
        private EditText confirmEdit;
        private TextView emailError;
        private TextView passwordError;
        private TextView confirmError;
        private Button loginBtn;
        private FirebaseApp firebaseApp;
        public static FirebaseAuth FirebaseAuth;
        public static FirebaseFirestore FirestoreDb { get; private set; }
        private const string userKey = "user";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_account);
            this.SetFirebaseApp();
            this.InitFields();
            this.InitFieldsActions();
            this.RegisterServices();
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
        }


        private void InitFields()
        {
            this.emailEdit = FindViewById<EditText>(Resource.Id.email_edit_text);
            this.passwordEdit = FindViewById<EditText>(Resource.Id.password_edit_text);
            this.confirmEdit = FindViewById<EditText>(Resource.Id.confirm_edit_text);
            this.emailError = FindViewById<TextView>(Resource.Id.email_error);
            this.passwordError = FindViewById<TextView>(Resource.Id.password_error);
            this.confirmError = FindViewById<TextView>(Resource.Id.confirm_error);
            this.loginBtn = FindViewById<Button>(Resource.Id.login_btn);
            var createAccountBtn = FindViewById<Button>(Resource.Id.login_btn);
            var resetBtn = FindViewById<Button>(Resource.Id.login_btn);
            var redirectText = FindViewById<TextView>(Resource.Id.already_have_account);
            redirectText.Click += (sender, args) =>
            {
                StartActivity(typeof(LoginActivity));
            };
            createAccountBtn.Click += CreateUser;
            resetBtn.Click += ResetForm;
        }

        private void InitFieldsActions()
        {
            this.EmailActions();
            this.PasswordActions();
            this.ConfirmActions();
        }

        private void EmailActions()
        {
            this.emailEdit.AfterTextChanged += (sender, args) =>
            {
                if (!this.emailEdit.Text.IsAValidEmail())
                    this.emailError.Visibility = ViewStates.Visible;
                else
                    this.emailError.Visibility = ViewStates.Invisible;
                if (this.IsValidLogin())
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
                if (this.IsValidLogin())
                {
                    this.loginBtn.Enabled = true;
                    this.loginBtn.SetBackgroundResource(Resource.Color.colorAccentDark);
                }
            };
        }

        private void ConfirmActions()
        {
            this.confirmEdit.AfterTextChanged += (sender, args) =>
            {
                if (this.passwordEdit?.Text != this.confirmEdit.Text)
                    this.confirmError.Visibility = ViewStates.Visible;
                else
                    this.confirmError.Visibility = ViewStates.Invisible;
                if (this.IsValidLogin())
                {
                    this.loginBtn.Enabled = true;
                    this.loginBtn.SetBackgroundResource(Resource.Color.colorAccentDark);
                }
            };   
        }

        private async void CreateUser(object sender, EventArgs e)
        {
            var creating = FindViewById<RelativeLayout>(Resource.Id.creating_account);
            creating.Visibility = ViewStates.Visible;
            if (this.IsValidLogin())
            {
                var email = this.emailEdit.Text;
                var password = this.passwordEdit.Text;
                try
                {
                    await FirebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
                    this.confirmError.Visibility = ViewStates.Invisible;
                    this.emailError.Visibility = ViewStates.Invisible;
                    this.passwordError.Visibility = ViewStates.Invisible;
                    creating.Visibility = ViewStates.Invisible;
                    StartActivity(typeof(MainActivity));
                }
                catch (FirebaseAuthException ex)
                {
                    Console.WriteLine(ex);
                    creating.Visibility = ViewStates.Invisible;
                }
            }
        }

        private void SetFirebaseApp()
        {
            var options = new FirebaseOptions.Builder()
                .SetProjectId(FirebaseUtils.projet_id)
                .SetApplicationId(FirebaseUtils.application_id)
                .SetApiKey(FirebaseUtils.api_key)
                .SetDatabaseUrl(FirebaseUtils.database_url)
                .SetStorageBucket(FirebaseUtils.storage_bucket)
                .Build();

            this.firebaseApp = FirebaseApp.InitializeApp(this, options);
            FirestoreDb = FirebaseFirestore.GetInstance(this.firebaseApp);
            FirebaseAuth = FirebaseAuth.GetInstance(this.firebaseApp);

        }

        private bool IsValidLogin()
        {
            if (
                this.emailEdit.Text.IsAValidEmail() &&
                this.passwordEdit.Text != System.String.Empty &&
                this.confirmEdit.Text != System.String.Empty &&
                this.passwordEdit.Text.Equals(this.confirmEdit.Text))
            {
                return true;
            }
            return false;
        }

        private void ResetForm(object sender, EventArgs e)
        {
            this.emailEdit.Text = this.passwordEdit.Text = this.confirmEdit.Text = System.String.Empty;
        }

        private void RegisterServices()
        {
            SimpleIoc.Default.Register<IMealPopup>(() => { return new AndroidMealPopup(); });
        }
    }
}
