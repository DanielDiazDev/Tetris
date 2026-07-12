using System;
using _Project.Scripts.Core;
using _Project.Scripts.MVC;
using _Project.Scripts.PlayfabServices;
using UniRx;

public class CurrencyModel : BaseModel
{
    // Add your data properties here
    public ReactiveProperty<int> Currency { get; } =  new ReactiveProperty<int>();

    private void Awake()
    {
        var currencyService = ServiceLocator.Instance.GetService<ICurrencyService>();
        currencyService.SetModel(this);
    }

    public void Init()
    {
        var currencyService = ServiceLocator.Instance.GetService<ICurrencyService>();
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