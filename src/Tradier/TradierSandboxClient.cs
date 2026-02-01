#nullable disable
namespace Tradier
{
    public class TradierSandboxClient : TradierClient
    {
        internal override string BaseAddress => "https://sandbox.tradier.com/v1/";
        internal override string StreamAddress => throw new NotImplementedException("Sandbox environment does not support streaming.");
        public TradierSandboxClient() : base() { }
        public TradierSandboxClient(HttpClient client) : base(client) {  }
        public TradierSandboxClient(TradierAuthentication auth) : base(auth) { }
        public TradierSandboxClient(HttpClient client, TradierAuthentication authentication) : base(client, authentication) { }
    }
}
#nullable restore
