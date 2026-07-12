using _Project.Scripts.MVC;
using UniRx;

public class CurrencyController : BaseController<CurrencyModel, CurrencyView>
{
    public CurrencyController(CurrencyView view, CurrencyModel model) : base(view, model)
    {
        // Subscribe to events here
        model.Currency.Subscribe(view.SetCurrency).AddTo(view);
    }


    ~CurrencyController()
    {
        // Unsubscribe to events here
    }
}                             