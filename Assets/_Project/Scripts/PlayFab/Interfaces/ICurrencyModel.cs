namespace _Project.Scripts.PlayFab.Interfaces
{
    public interface ICurrencyModel
    {
        void AddCurrency(int amount);
        void SubtractCurrency(int amount);
        void Init();
    }
}
