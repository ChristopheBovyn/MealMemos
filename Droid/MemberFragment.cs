using System;
using System.ComponentModel;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using MealMemos.Models;
using Xamarin.Essentials;

namespace MealMemos.Droid
{
    public class MemberFragment : Android.Support.V4.App.Fragment
    {
        private const string MEMBER_FIRSTNAME = "member_firstname";
        private static string BACKGROUND_COLOR = "background_color";

        public static MemberFragment NewInstance(Member member)
        {
            MemberFragment memberFragment = new MemberFragment();
            Bundle args = new Bundle();
            args.PutString(MEMBER_FIRSTNAME, member.Firstname);
            args.PutString(BACKGROUND_COLOR, member.ColorTheme);
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
            var addButton = view.FindViewById<FloatingActionButton>(Resource.Id.addElement);
            addButton.Click += (sender, args) =>
            {
               var result = SimpleIoc.Default.GetInstance<IMemberPopup>().OpenPopupWithResult().Result;
                this.SetInfo(view, container, result);
            };
            return view;
        }

        private void SetInfo(View view, ViewGroup container,string info)
        {
            TableLayout stack = view.FindViewById<TableLayout>(Resource.Id.memberInfos);
            var infoTextView = new TextView(container.Context)
            {
                Text = info
            };
            stack.AddView(infoTextView);
        }
    }
}
