namespace Application.Common.Exceptions
{
    public class ConflictException : ApplicationExceptionBase
    {
        public ConflictException(string message) : base(message) { }
    }
}
