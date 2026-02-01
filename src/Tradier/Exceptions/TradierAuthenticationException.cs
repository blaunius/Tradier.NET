namespace Tradier
{
    public class TradierAuthenticationException : System.Exception
    {
        public TradierAuthenticationException() : base("An Authentication Error has occurred")
        {
        }
        public TradierAuthenticationException(string message) : base(message)
        {
        }
    }
}
