using System;
using System.Collections.Generic;

namespace MealMemos.Models
{
    public class MealItem
    {
        public const string Breakfast = "Breakfast";
        public const string Diner = "Diner";
        public const string Souper = "Souper";
        public const string Collation = "Collation";

        public string mealTitle;

        public MealItem(string mealTitle)
        {
            this.mealTitle = mealTitle;
        }

        public static List<MealItem> GetDefaultMeals()
        {
            List<MealItem> mealItems = new List<MealItem>();
            mealItems.Add(new MealItem(MealItem.Breakfast));
            mealItems.Add(new MealItem(MealItem.Diner));
            mealItems.Add(new MealItem(MealItem.Souper));
            mealItems.Add(new MealItem(MealItem.Collation));
            return mealItems;
        }
    }
}
