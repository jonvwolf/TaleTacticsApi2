namespace HorrorTacticsApi2.Domain.Exceptions
{
    public class HtBadRequestException : HtException
    {
        public HtBadRequestException(string msg) : base(msg)
        {
            
        }

        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
