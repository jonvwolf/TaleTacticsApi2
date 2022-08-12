namespace HorrorTacticsApi2.Domain.Exceptions
{
    public class HtConflictException : HtException
    {
        public HtConflictException(string msg) : base(msg)
        {
        }

        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}
