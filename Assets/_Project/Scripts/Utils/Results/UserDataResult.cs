namespace _Project.Scripts.Utils.Results
{
    public class UserDataResult
    {
        public bool Success { get; }
        public string Message { get; }
        public object RawData { get; }

        public UserDataResult(bool success, string message = null, object rawData = null)
        {
            Success = success;
            Message = message;
            RawData = rawData;
        }
    }

    public class UserDataResult<T> : UserDataResult
    {
        public T Data { get; }

        public UserDataResult(bool success, T data, string message = null, object rawData = null)
            : base(success, message, rawData)
        {
            Data = data;
        }
    }
}