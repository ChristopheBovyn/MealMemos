using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace MealMemos.Droid
{
    public class MemberFragment : Android.Support.V4.App.Fragment
    {
        private const string MEMBER_FIRSTNAME = "member_firstname";
        private static string BACKGROUND_COLOR = "background_color";
        public MemberFragment()
        {
        }

        public static MemberFragment newInstance(String firstname, string backgroundColor)
        {
            MemberFragment memberFragment = new MemberFragment();
            Bundle args = new Bundle();
            if (backgroundColor != "") { args.PutString(BACKGROUND_COLOR, backgroundColor);}
            args.PutString(MEMBER_FIRSTNAME,firstname);
            memberFragment.Arguments = args;
            return memberFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            string firstname = Arguments.GetString(MEMBER_FIRSTNAME, "");
            string bgcolor = Arguments.GetString(BACKGROUND_COLOR, "#000000");
            View view = inflater.Inflate(Resource.Layout.member, container, false);
            TextView firstnameTextView = (TextView)view.FindViewById(Resource.Id.member_firstname);
            firstnameTextView.Text = firstname;
            view.SetBackgroundColor(ColorConverters.FromHex(bgcolor).ToPlatformColor());
            return view;
        }
    }
}
