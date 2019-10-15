using Android.Content;
using Android.Support.V4.App;
using Java.Lang;
using MealMemos.Models;

namespace MealMemos.Droid
{
    public class TabsAdapter : FragmentStatePagerAdapter
    {
        Team team;

        public TabsAdapter(FragmentManager fm) : base(fm)
        {
            this.team = new Team();
        }
        public override int Count => team.MemberCount;

        public override Fragment GetItem(int position)
        {
            return MemberFragment.NewInstance(team[position]);
        }

        public override ICharSequence GetPageTitleFormatted(int position) =>
                            new String("Member "+(position+1));
    }
}
