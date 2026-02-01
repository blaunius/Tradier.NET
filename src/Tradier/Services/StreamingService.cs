using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tradier.Services
{
    public class StreamingService : TradierService
    {
        public StreamingService() : base()
        {
            if (this.client is TradierSandboxClient)
                throw new NotSupportedException("Streaming information can only be used in the production client.");
        }
        public StreamingService(ITradierClient tradierClient) : base(tradierClient)
        {
            if (tradierClient is TradierSandboxClient)
                throw new NotSupportedException("Streaming information can only be used in the production client.");
        }
    }
}
