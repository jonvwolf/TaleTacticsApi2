namespace HorrorTacticsApi2.Domain.Exceptions
{
    public class HtNotFoundException : HtException
    {
        public HtNotFoundException(string msg) : base(msg)
        {
        }

        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}
