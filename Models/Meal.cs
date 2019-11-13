using System;
using System.Collections.Generic;

namespace MealMemos.Models
{
    public class MealDocument
    {
        List<string> Breakfast = new List<string>();
        List<string> Diner = new List<string>();
        List<string> Souper = new List<string>();
        List<string> Collation = new List<string>();
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

        public static MealDocument DataToMealDocument(IDictionary<string,object> data)
        {
            MealDocument mealDocument = new MealDocument();
            foreach(var element in data)
            {
                switch (element.Key){
                    case "user":
                        mealDocument.user = (string)element.Value;
                        break;
                    case "date":
                        mealDocument.date = (string)element.Value;
                        break;
                    case "Breakfast":
                        var test = element.Value;
                        break;

                }
            }
            return mealDocument;
        }
    }
}
