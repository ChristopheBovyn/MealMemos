using System;
using System.Threading.Tasks;

namespace MealMemos.Interfaces
{
    public interface IMealPopup
    {
        void OpenPopup();
        Task<string> OpenPopupWithResult();
    }
}
