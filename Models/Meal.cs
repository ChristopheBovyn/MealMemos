using System;
using System.Collections.Generic;

namespace MealMemos.Models
{
    public class MealDocument
    {
        public List<string> Breakfast{ get; set; } = new List<string>();
        public List<string> Diner{ get; set; } = new List<string>();
        public List<string> Souper{ get; set; } = new List<string>();
        public List<string> Collation{ get; set; } = new List<string>();
        public static string UserKey = "user";
        public static string DateKey = "date";
        private String date;
        private String user;
        public MealDocument(String user, String date)
        {
            this.user = user;
            this.date = date;
        }

        public MealDocument() { }

        public void SetMeal(List<string> mealContent, string mealTitle)
        {
            switch (mealTitle)
            {
                case "Breakfast":
                    this.Breakfast.AddRange(mealContent);
                    break;
                case "Diner":
                    this.Diner.AddRange(mealContent);
                    break;
                case "Souper":
                    this.Souper.AddRange(mealContent);
                    break;
                case "Collation":
                    this.Collation.AddRange(mealContent);
                    break;
            }
        }
    }
}
