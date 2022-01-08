namespace HorrorTacticsApi2.Domain.Models
{
    public class TokenModel
    {
        public string Token { get; set; } = "";

        public TokenModel()
        {

        }

        public TokenModel(string token)
        {
            Token = token;
        }
    }
}
