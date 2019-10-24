using System;
using System.Threading.Tasks;

namespace MealMemos.Interfaces
{
    public interface IMealPopup
    {
        Task<string> OpenPopupWithResult();
    }
}
