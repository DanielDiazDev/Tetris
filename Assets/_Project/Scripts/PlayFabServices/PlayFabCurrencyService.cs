using System;
using System.Collections.Generic;
using _Project.Scripts.Core;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace _Project.Scripts.PlayfabServices
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
        void SetModel(CurrencyModel currencyModel);
        CurrencyModel CurrencyModel { get; }
    }

    public class PlayFabCurrencyService : ICurrencyService, IDisposable
    {
        private readonly Dictionary<string, int> currencyBalances = new();
        private CurrencyModel model;
        public CurrencyModel CurrencyModel => model;
        public void SetModel(CurrencyModel currencyModel)
        {
            model = currencyModel;
        }
        public void AddCurrency(string currencyCode, int amount, 
            Action<int> onSuccess = null, 
            Action<string> onError = null)
        {
            var request = new AddUserVirtualCurrencyRequest
            {
                Amount = amount,
                VirtualCurrency = currencyCode
            };

            PlayFabClientAPI.AddUserVirtualCurrency(request,
                result =>
                {
                    Debug.Log(result.BalanceChange);
                    currencyBalances[currencyCode] = result.Balance;
                    model.AddCurrency(result.BalanceChange);
                    onSuccess?.Invoke(result.BalanceChange);
                },
                error => onError?.Invoke(error.GenerateErrorReport()));
        }
        public void SubtractCurrency(string currencyCode, int amount,
            Action<int> onSuccess = null,
            Action<string> onError = null)
        {
            var request = new SubtractUserVirtualCurrencyRequest()
            {
                Amount = amount,
                VirtualCurrency = currencyCode
            };
            PlayFabClientAPI.SubtractUserVirtualCurrency(request, result =>
            {
                currencyBalances[currencyCode] = result.Balance;
                model.SubtractCurrency(result.BalanceChange);
                onSuccess?.Invoke(result.BalanceChange);
            },
                error => onError?.Invoke(error.GenerateErrorReport()));
        }

        public void GetBalance(string currencyCode,
            Action<int> onSuccess = null,
            Action<string> onError = null)
        {
            if (currencyBalances.TryGetValue(currencyCode, out var balance))
            {
                onSuccess?.Invoke(balance);
                return;
            }
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
            {
                if (result.VirtualCurrency.TryGetValue(currencyCode, out var currentBalance))
                {
                    currencyBalances[currencyCode] = currentBalance;
                    onSuccess?.Invoke(currentBalance);
                }
            },
            error => onError?.Invoke(error.GenerateErrorReport()));
        }

        public void Dispose()
        {
            ServiceLocator.Instance.UnregisterService<ICurrencyService>();
        }
    }
    
}