using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using MealMemos.Interfaces;
using MealMemos.Models;

namespace MealMemos.ViewModels
{
    public class MealViewModel
    {
        private IMealAPI mealAPI;

        public DateTime Date { get; set; }
        public string UserID { get; set; }

        public MealDocument MealDocument { get; set; }

        public string ApiFormattedDateTime => this.Date.ToString("yyyy-MM-dd");

        public Action OnMealDocumentChanged { get; set; }

        public MealViewModel(string userId, DateTime date)
        {
            this.mealAPI = SimpleIoc.Default.GetInstance<IMealAPI>();
            this.UserID = userId;
            this.Date = date;
            this.MealDocument = new MealDocument();
        }

        public async Task LoadDishesAsync()
        {
            this.MealDocument = await this.mealAPI.LoadDishesAsync(this.Date, this.UserID);
            this.OnMealDocumentChanged?.Invoke();
        }

        public void AddDish(string dish, string mealTitle)
        {
            this.MealDocument.AddDish(dish, mealTitle);
        }

        public void RemoveDishAt(int index, string mealTitle)
        {
            this.MealDocument.RemoveDishAt(index, mealTitle);
        }

        public void SaveDishes(string mealTitle)
        {
            if(this.MealDocument?.DocumentId == String.Empty)
            {
                this.MealDocument.DocumentId = this.mealAPI.CreateDocument(this.Date, this.UserID);
            }
            this.mealAPI.SaveDishes(this.MealDocument?.DocumentId,mealTitle,this.GetMealByName(mealTitle));
        }

        public List<string> GetMealByName(string mealTitle)
        {
            return this.MealDocument.GetMeal(mealTitle);
        }
    }
}
