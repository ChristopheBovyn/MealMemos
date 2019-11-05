using System;
using System.Collections.Generic;

namespace MealMemos.Models
{
    public class Meal
    {
        public  List<string> Breakfast { get; }
        public List<string> Diner { get; }
        public List<string> Souper { get; }
        public List<string> Collation { get; }
        public DateTime Date;
        public Meal()
        {
        }
    }
}
