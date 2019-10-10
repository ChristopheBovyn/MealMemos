using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;
using MealMemos.Models;

namespace MealMemos.Droid
{
    public class TeamAdapter : FragmentPagerAdapter
    {
        public Team team;
        public TeamAdapter(FragmentManager fm, Team team, List<string> backgroundColors) : base(fm)
        {
            this.team = team;
        }

        public override int Count
        {
            get { return this.team.MemberCount; }
        }

        public override Fragment GetItem(int position)
        {
            if(position  < team.MemberCount)
            {
                return MemberFragment.NewInstance(this.team[position]);
            }
            return null;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String("Member "+(position+1));
        }
    }
}
