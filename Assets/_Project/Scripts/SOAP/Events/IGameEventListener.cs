namespace _Project.Scripts.SOAP.Events
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T data);
    }
}