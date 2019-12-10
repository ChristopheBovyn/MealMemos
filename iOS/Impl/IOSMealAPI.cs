using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using Foundation;
using MealMemos.Interfaces;
using MealMemos.Models;

namespace MealMemos.iOS.Impl
{
    public class IOSMealAPI : IMealAPI
    {
        private const string DateTimeFormat = "yyyy-MM-dd";
        private const string dateKey = "date";
        private const string userKey = "user";
        private const string collection = "meals";

        private readonly List<string> defaultMeals = new List<string>
        {
            "Breakfast",
            "Diner",
            "Souper",
            "Collation"
        };

        public string CreateDocument(DateTime date, string userId)
        {
            var document = Firestore.SharedInstance.GetCollection(collection).CreateDocument();
            NSString userNS = new NSString(userKey);
            NSString dateNS = new NSString(dateKey);
            var contentDictionary = new NSMutableDictionary<NSString, NSObject>();
            contentDictionary.SetValueForKey(NSObject.FromObject(date.Date.ToString(DateTimeFormat)), dateNS);
            contentDictionary.SetValueForKey(NSObject.FromObject(userId), userNS);
            var mealDictionary = new NSDictionary<NSString, NSObject>(contentDictionary.Keys, contentDictionary.Values);
            document.SetDataAsync(mealDictionary);
            return document.Id;
        }

        public void SaveDishes(string documentId, string mealTitle, List<string> dishes)
        {
            var content = new NSMutableDictionary();
            for (int i = 0; i < dishes.Count; i++)
            {
                content.Add(new NSString("" + (i + 1)), NSObject.FromObject(dishes[i]));
            }
            var doc = Firestore.SharedInstance.GetCollection(collection).GetDocument(documentId);
            var data = new Dictionary<object, object>();
            data.Add(mealTitle, content);
            doc.UpdateDataAsync(data);
        }

        public async Task<MealDocument> LoadDishesAsync(DateTime date, string userId)
        {
            var documentQuery = await Firestore.SharedInstance
                                              .GetCollection(collection)
                                              .WhereEqualsTo(userKey, userId)
                                              .WhereEqualsTo(dateKey, date.Date.ToString(DateTimeFormat))
                                              .LimitedTo(1)
                                              .GetDocumentsAsync();

            var result = documentQuery?.Documents?.FirstOrDefault();
            if (result != null)
            {
                return this.FirebaseDocumentToMealDocument(result);
            }

            return new MealDocument();
        }

        private MealDocument FirebaseDocumentToMealDocument(DocumentSnapshot document)
        {
            MealDocument mealDocument = new MealDocument();
            mealDocument.DocumentId = document.Id;
            foreach (var data in document?.Data)
            {
                var mealContent = new List<string>();
                if (this.defaultMeals.Contains(data.Key?.ToString()))
                {
                    if (data.Value is NSMutableDictionary dictionary)
                    {
                        foreach (var element in dictionary)
                        {
                            mealContent.Add(element.Value.ToString());
                        }
                        mealDocument.SetMeal(mealContent, data.Key?.ToString());
                    }
                }
            }
            return mealDocument;
        }
    }
}
