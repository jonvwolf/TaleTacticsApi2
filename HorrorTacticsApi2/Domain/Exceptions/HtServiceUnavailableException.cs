namespace HorrorTacticsApi2.Domain.Exceptions
{
    public class HtServiceUnavailableException : HtException
    {
        public HtServiceUnavailableException(string msg) : base(msg)
        {
        }

        public override int StatusCode => StatusCodes.Status503ServiceUnavailable;
    }
}
