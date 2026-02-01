using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tradier.Services
{
    public class StreamingService : TradierService
    {
        /// <summary>
        /// Creates a new StreamingService with the specified client.
        /// </summary>
        /// <param name="tradierClient">The Tradier API client to use (must be production client, not sandbox).</param>
        /// <exception cref="NotSupportedException">Thrown when attempting to use with a sandbox client.</exception>
        public StreamingService(ITradierClient tradierClient) : base(tradierClient)
        {
            if (tradierClient is TradierSandboxClient)
                throw new NotSupportedException("Streaming information can only be used in the production client.");
        }
    }
}
