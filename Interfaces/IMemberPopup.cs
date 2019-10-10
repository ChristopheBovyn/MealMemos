using System;
using System.Threading.Tasks;

namespace MealMemos.Interfaces
{
    public interface IMemberPopup
    {
        void OpenPopup();
        Task<string> OpenPopupWithResult();
    }
}
