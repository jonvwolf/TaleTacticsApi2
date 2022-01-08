namespace HorrorTacticsApi2.Domain.Exceptions
{
    public class HtBadRequestException : HtException
    {
        public HtBadRequestException(string msg) : base(msg)
        {
            // TODO: this should be converted to a problem? with the details and such
        }
    }
}
