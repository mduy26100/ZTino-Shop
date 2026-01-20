namespace Application.Common.Exceptions
{
    public class ConcurrencyException : ApplicationExceptionBase
    {
        public ConcurrencyException(string message) : base(message)
        {
        }
    }
}
