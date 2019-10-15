﻿using System;
using System.ComponentModel;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using MealMemos.Models;
using Xamarin.Essentials;
using MealMemos.Extensions;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Android.Support.V7.Widget;
using Org.Xml.Sax.Helpers;
using System.Collections.Generic;

namespace MealMemos.Droid
{
    public class MemberFragment : Android.Support.V4.App.Fragment
    {
        private const string MEMBER_FIRSTNAME = "member_firstname";
        private static string BACKGROUND_COLOR = "background_color";

        private TextView firstnameTextView;
        private RecyclerView recyclerView;

        private InformationAdapter informationAdapter;

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
            this.recyclerView = view.FindViewById<RecyclerView>(Resource.Id.memberInfos);
            this.recyclerView.SetLayoutManager(new LinearLayoutManager(Application.Context));
            this.informationAdapter = new InformationAdapter(new List<string>());
            this.recyclerView.SetAdapter(informationAdapter);

            this.firstnameTextView = (TextView)view.FindViewById(Resource.Id.member_firstname);
            this.firstnameTextView.Text = firstname;

            view.SetBackgroundColor(ColorConverters.FromHex(bgcolor).ToPlatformColor());
            var addButton = view.FindViewById<FloatingActionButton>(Resource.Id.addElement);
            addButton.Click += this.AddButtonClick;
            return view;
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            this.OpenPopup().SafeFireAndForget();
        }

        private async Task OpenPopup()
        {
            var result = await SimpleIoc.Default.GetInstance<IMemberPopup>().OpenPopupWithResult();
            if (!result.IsNullOrEmpty())
            {
                this.SetInfo(result);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("MemberFragment : AddButtonClick() : result is null");
            }
        }

        private void SetInfo(string info)
        {
            this.informationAdapter.addInformation(info);
        }
    }
}
