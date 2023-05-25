namespace DemoToken.CustomResult
{
    public class CustomStatusCode<T>
    {
        public CustomStatusCode()
        {
        }

        public CustomStatusCode(int status, string message, T? data, string? error)
        {
            StatusCode = status;
            StatusMessage = message;
            Data = data;
            Error = error;
        }

        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
    }
}