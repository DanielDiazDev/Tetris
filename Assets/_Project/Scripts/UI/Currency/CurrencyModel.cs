using System;
using _Project.Scripts.MVC;
using _Project.Scripts.PlayFab.Interfaces;
using UniRx;
using UnityServiceLocator;

public class CurrencyModel : BaseModel, ICurrencyModel
{
    // Add your data properties here
    public ReactiveProperty<int> Currency { get; } =  new ReactiveProperty<int>();

    private void Awake()
    {
        var currencyService = ServiceLocator.Global.Get<ICurrencyService>();
        currencyService.SetModel(this);
        Init();
    }

    public void Init()
    {
        if(!ServiceLocator.Global.Get<ILogin>().IsInitialized) return;
        var currencyService = ServiceLocator.Global.Get<ICurrencyService>();
        currencyService.GetBalance("SC", value =>
        {
            Currency.Value = value;
        });
    }

    public void AddCurrency(int  amount)
    {
        Currency.Value += amount;
    }

    public void SubtractCurrency(int amount)
    {
        Currency.Value -= amount;
    }
}