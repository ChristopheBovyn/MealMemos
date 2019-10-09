using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;
using MealMemos.Models;

namespace MealMemos.Droid
{
    public class TeamAdapter : FragmentPagerAdapter
    {
        public Team team;

        private List<string> backgroundColors;
        public TeamAdapter(FragmentManager fm, Team team, List<string> backgroundColors) : base(fm)
        {
            this.team = team;
            this.backgroundColors = backgroundColors;
        }

        public override int Count
        {
            get { return this.team.MemberCount; }
        }

        public override Fragment GetItem(int position)
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
            return MemberFragment.newInstance(memberFName, color);
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String("Member "+(position+1));
        }
    }
}
