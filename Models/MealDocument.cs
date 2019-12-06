using System.Collections.Generic;

namespace MealMemos.Models
{
    public class MealDocument
    {
        public List<string> Breakfast{ get; set; } = new List<string>();
        public List<string> Diner{ get; set; } = new List<string>();
        public List<string> Souper{ get; set; } = new List<string>();
        public List<string> Collation{ get; set; } = new List<string>();
        public string DocumentId { get; set; } = string.Empty;
        public static string UserKey = "user";
        public static string DateKey = "date";
        

        public void SetMeal(List<string> mealContent, string mealTitle)
        {
            switch (mealTitle)
            {
                case "Breakfast":
                    this.Breakfast = mealContent;
                    break;
                case "Diner":
                    this.Diner = mealContent;
                    break;
                case "Souper":
                    this.Souper = mealContent;
                    break;
                case "Collation":
                    this.Collation = mealContent;
                    break;
            }
        }

        public List<string> GetMeal(string mealTitle)
        {
            switch (mealTitle)
            {
                case "Breakfast":
                    return this.Breakfast;
                case "Diner":
                    return this.Diner;
                case "Souper":
                    return this.Souper;
                case "Collation":
                    return this.Collation;
                default:
                    return new List<string>();
            }
        }

        public void AddDish(string dish,string mealTitle)
        {
            switch (mealTitle)
            {
                case "Breakfast":
                    this.Breakfast.Add(dish);
                    break;
                case "Diner":
                    this.Diner.Add(dish);
                    break;
                case "Souper":
                    this.Souper.Add(dish);
                    break;
                case "Collation":
                    this.Collation.Add(dish);
                    break;
            }
        }

        public void RemoveDishAt(int index,string mealTitle)
        {
            switch (mealTitle)
            {
                case "Breakfast":
                    this.Breakfast.RemoveAt(index);
                    break;
                case "Diner":
                    this.Diner.RemoveAt(index);
                    break;
                case "Souper":
                    this.Souper.RemoveAt(index);
                    break;
                case "Collation":
                    this.Collation.RemoveAt(index);
                    break;
            }
        }
    }
}
