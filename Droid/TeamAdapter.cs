using Android.Support.V4.App;
using Xamarin.Essentials;
using MealMemos.Models;
using System.Collections.Generic;
using Java.Lang;
using System.Linq;

namespace MealMemos.Droid
{
    public class TeamAdapter : FragmentPagerAdapter
    {
        public Team team;

        private List<string> backgroundColors;
        public TeamAdapter(Android.Support.V4.App.FragmentManager fm, Team team, List<string> backgroundColors) : base(fm)
        {
            this.team = team;
            this.backgroundColors = backgroundColors;
        }

        public override int Count
        {
            get { return this.team.MemberCount; }
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            string memberFName = "";
            string color = "";
            if(position  < team.MemberCount)
            {
                memberFName = this.team[position].Firstname;
            }
            if(position < this.backgroundColors.Count)
            {
                color = this.backgroundColors[position];
            }
            return (Android.Support.V4.App.Fragment)
               MemberFragment.newInstance(memberFName, color);
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String("Member "+(position+1));
        }
    }
}
