using System;
using System.Collections.Generic;

namespace MealMemos
{
    public class Member
    {
        public string Firstname { get; }

        public Member(string firstname)
        {
            Firstname = firstname;
        }
    }

    public class Team
    {
        private Member[] teamMembers;

        static Member[] defaultMembers =
        {
            new Member("Jordy"),
            new Member("Adrien"),
            new Member("Benjamin"),
            new Member("Jerome"),
            new Member("Sofiane"),
            new Member("Christophe")
        };

        public Team()
        {
            teamMembers = defaultMembers;
        }

        public int MemberCount { get { return teamMembers.Length; } }

        public Member this[int i] { get { return teamMembers[i]; } }
    }
}
