namespace HorrorTacticsApi2.Domain.Exceptions
{
    public abstract class HtException : Exception
    {
        public abstract int StatusCode { get; }
        public HtException(string msg) : base(msg)
        {

        }
    }
}
