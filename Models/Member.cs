using System;
using System.Collections.Generic;

namespace MealMemos.Models
{
    public class Member
    {
        public string Firstname { get; }
        public string ColorTheme { get; }

        public Member(string firstname)
        {
            Firstname = firstname;
        }

        public Member(string firstname,String color)
        {
            Firstname = firstname;
            ColorTheme = color;
        }
    }

    public class Team
    {
        private Member[] teamMembers;

        static Member[] defaultMembers =
        {
            new Member("Jordy",Color.Blue),
            new Member("Adrien",Color.Gray),
            new Member("Benjamin",Color.Yellow),
            new Member("Jerome",Color.Red),
            new Member("Sofiane",Color.Magenta),
            new Member("Christophe",Color.Purple)
        };

        public Team()
        {
            teamMembers = defaultMembers;
        }

        public int MemberCount { get { return teamMembers.Length; } }

        public Member this[int i] { get { return teamMembers[i]; } }
    }
}
