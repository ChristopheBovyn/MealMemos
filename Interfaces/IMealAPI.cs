using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MealMemos.Models;

namespace MealMemos.Interfaces
{
    public interface IMealAPI
    {
        Task<MealDocument> LoadDishesAsync(DateTime date, string userId);
        void SaveDishes(string documentId, string mealTitle,List<string> dishes);
        string CreateDocument(DateTime date, string userId);
    }
}
