using System;

namespace _Project.Scripts.PlayFab.Interfaces
{
    public interface ICurrencyService
    {
        void AddCurrency(string currencyCode, int amount, 
            Action<int> onSuccess = null, 
            Action<string> onError = null);

        void SubtractCurrency(string currencyCode, int amount,
            Action<int> onSuccess = null,
            Action<string> onError = null);

        void GetBalance(string currencyCode,
            Action<int> onSuccess = null,
            Action<string> onError = null);

        void Dispose();
        void SetModel(ICurrencyModel currencyModel);
        ICurrencyModel CurrencyModel { get; }
    }
}