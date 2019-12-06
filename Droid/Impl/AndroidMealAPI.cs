using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Android.Runtime;
using Firebase.Firestore;
using Java.Util;
using MealMemos.Interfaces;
using MealMemos.Models;

namespace MealMemos.Droid.Impl
{
    public class AndroidMealAPI : IMealAPI
    {
        private const string DateTimeFormat = "yyyy-MM-dd";

        private readonly List<string> defaultMeals = new List<string>
        {
            "Breakfast",
            "Diner",
            "Souper",
            "Collation"
        };

        private const string dateKey = "date";
        private const string userKey = "user";
        private const string collection = "meals";

        public async Task<MealDocument> LoadDishesAsync(DateTime date, string userId)
        {
            var result = await CreateAccountActivity.FirestoreDb.Collection(collection)
                             .WhereEqualTo(userKey, userId)
                             .WhereEqualTo(dateKey, date.Date.ToString(DateTimeFormat))
                             .Get();
            return this.ParseFirebaseResultToMealDocument(result);
        }

        public void SaveDishes(string documentId, string mealTitle, List<String> dishes)
        {
            HashMap content = new HashMap();
            for (int i = 0; i < dishes.Count; i++)
            {
                content.Put("" + (i + 1), dishes[i]);
            }
            DocumentReference document = CreateAccountActivity.FirestoreDb
                                                         .Collection(collection)
                                                         .Document(documentId);
            document.Update(mealTitle, content);

        }

        private MealDocument ParseFirebaseResultToMealDocument(object result)
        {
            var snapshot = (QuerySnapshot)result;
            if (!snapshot.IsEmpty)
            {
                var document = snapshot.Documents[0];
                MealDocument mealDocument = new MealDocument();
                mealDocument.DocumentId = document.Id;
                foreach (var element in document.Data)
                {
                    var key = element.Key;
                    if (this.defaultMeals.Contains(key))
                    {
                        var listDish = new List<string>();
                        var content = document.Get(key);

                        var dictio = new JavaDictionary<string, string>(content.Handle, JniHandleOwnership.DoNotRegister);

                        foreach (KeyValuePair<string, string> item in dictio)
                        {
                            listDish.Add(item.Value);
                        }
                        mealDocument.SetMeal(listDish, key);
                    }
                }
                return mealDocument;
            }
            return new MealDocument();
        }

        public String CreateDocument(DateTime date, string user)
        {
            // Check dateTime
            DocumentReference document = CreateAccountActivity.FirestoreDb.Collection(collection).Document();
            HashMap data = new HashMap();
            data.Put(userKey, user);
            data.Put(dateKey, date.Date.ToString(DateTimeFormat));
            document.Set(data);
            return document.Id;
        }
    }
}