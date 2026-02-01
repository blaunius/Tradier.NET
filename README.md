<div align="center">

# Tradier.Client

### A Modern .NET SDK for the Tradier Brokerage API

[![NuGet Version](https://img.shields.io/nuget/v/Tradier.Client?style=flat-square&logo=nuget&color=004880)](https://www.nuget.org/packages/Tradier.Client)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Tradier.Client?style=flat-square&logo=nuget&color=004880)](https://www.nuget.org/packages/Tradier.Client)
[![License](https://img.shields.io/github/license/blaunius/Tradier.NET?style=flat-square&color=blue)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)

Build powerful trading applications with real-time market data, options chains, and order execution.

[Installation](#installation) •
[Quick Start](#quick-start) •
[Services](#available-services) •
[Examples](#examples) •
[License](#license)

</div>

---

## ✨ Features

- **Multi-Framework Support** — .NET 8, .NET 9, and .NET 10
- **Complete API Coverage** — Market data, trading, accounts, watchlists, and streaming
- **Options Trading** — Full options chains with Greeks (Delta, Gamma, Theta, Vega, IV)
- **Production Ready** — Battle-tested with comprehensive error handling
- **Async/Await** — Fully asynchronous API for optimal performance
- **Dependency Injection** — First-class ASP.NET Core integration
- **Paper Trading** — Sandbox environment for risk-free testing
- **Thread-Safe** — No static state, safe for multi-tenant applications

---

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package Tradier.Client
```

Or via the Package Manager Console:

```powershell
Install-Package Tradier.Client
```

---

## Quick Start

### 1. Get Your API Credentials

1. Create an account at [tradier.com](https://tradier.com)
2. Navigate to [API Settings](https://dash.tradier.com/settings/api)
3. Generate your API token (sandbox for testing, production for live trading)

### 2. Initialize the Client

**Console / Desktop Applications:**

```csharp
using Tradier;
using Tradier.Services;

// Create authentication with your api key
var auth = new TradierAuthentication("YOUR_API_KEY");

// Create client (use TradierSandboxClient for paper trading)
var client = new TradierSandboxClient(auth);

// Use the services
var marketData = new MarketDataService(client);
var quotes = await marketData.GetQuotes(true, "AAPL", "MSFT", "GOOGL");
```

**ASP.NET Core Applications:**

```csharp
// Program.cs
using Tradier.Extensions;

// One-liner setup (defaults to sandbox for safety)
builder.Services.AddTradier("YOUR_API_KEY");

// Or with configuration options
builder.Services.AddTradier(options =>
{
    options.ApiKey = builder.Configuration["Tradier:ApiKey"]!;
    options.UseSandbox = builder.Environment.IsDevelopment();
});
```

```csharp
// Inject services directly into your controllers
public class MarketController : ControllerBase
{
    private readonly MarketDataService _market;

    public MarketController(MarketDataService market) => _market = market;

    [HttpGet("quote/{symbol}")]
    public async Task<IActionResult> GetQuote(string symbol)
    {
        var result = await _market.GetQuotes(true, symbol);
        return result.IsSuccessful ? Ok(result.Data) : BadRequest(result.ErrorMessage);
    }
}
```

### 3. Fetch Market Data

```csharp
var marketData = new MarketDataService(client);

// Get real-time quotes
var quotes = await marketData.GetQuotes(true, "AAPL", "MSFT", "GOOGL");

// Get options chain with Greeks
var chain = await marketData.GetOptionChains("AAPL", expirationDate, includeAllGreeks: true);
```

---

## Available Services

| Service | Description |
|---------|-------------|
| `MarketDataService` | Real-time quotes, options chains, historical data, market calendar |
| `AccountService` | Balances, positions, order history, gain/loss reports |
| `TradingService` | Equity and options order placement, modifications, cancellations |
| `WatchlistService` | Create, update, and manage watchlists |
| `StreamingService` | Real-time streaming quotes and events (production only) |

### Client Types

| Client | Environment | Use Case |
|--------|-------------|----------|
| `TradierSandboxClient` | sandbox.tradier.com | Paper trading & development |
| `TradierClient` | api.tradier.com | Live trading with real funds |

> ⚠️ **Important:** Always test thoroughly with the sandbox environment before using production credentials.

---

## Examples

### Fetching Options Data

```csharp
var auth = new TradierAuthentication("YOUR_API_KEY");
var client = new TradierSandboxClient(auth);
var marketData = new MarketDataService(client);

// Get available expiration dates
var expirations = await marketData.GetOptionExpirations("AAPL");
var nextExpiration = expirations.Data?.Dates?.First();

// Fetch the full options chain with Greeks
var chain = await marketData.GetOptionChains(
    symbol: "AAPL",
    expiration: DateTime.Parse(nextExpiration),
    includeAllGreeks: true
);

// Process the data
foreach (var option in chain.Data?.Options ?? [])
{
    Console.WriteLine($"{option.Symbol}");
    Console.WriteLine($"  Strike: ${option.Strike}");
    Console.WriteLine($"  Bid/Ask: ${option.Bid} / ${option.Ask}");
    Console.WriteLine($"  Delta: {option.Greeks?.Delta:F4}");
    Console.WriteLine($"  IV: {option.Greeks?.MidIv:P2}");
}
```

### Placing an Order

```csharp
var auth = new TradierAuthentication("YOUR_API_KEY");
var client = new TradierSandboxClient(auth);
var trading = new TradingService(client);

// Place a limit order
var order = await trading.PlaceEquityOrder(
    accountId: "YOUR_ACCOUNT_ID",
    symbol: "AAPL",
    side: OrderSide.Buy,
    quantity: 10,
    type: OrderType.Limit,
    price: 150.00m,
    duration: OrderDuration.Day
);

if (order.IsSuccessful)
{
    Console.WriteLine($"Order placed: {order.Data?.Id}");
}
```

### Error Handling

```csharp
var result = await marketData.GetQuotes(true, "AAPL");

if (result.IsSuccessful)
{
    // Access the data
    var quote = result.Data;
}
else
{
    // Handle the error
    Console.WriteLine($"Error: {result.ErrorMessage}");
    Console.WriteLine($"Status: {result.StatusCode}");
}
```

---

## Configuration

### Using User Secrets (Recommended for Development)

```bash
dotnet user-secrets set "Tradier:ApiKey" "your-token-here"
```

```csharp
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var auth = new TradierAuthentication(config["Tradier:ApiKey"]!);
var client = new TradierSandboxClient(auth);
```

### Using Custom HttpClient

For advanced scenarios (proxies, custom handlers, etc.):

```csharp
var httpClient = new HttpClient();
// Configure httpClient as needed...

var auth = new TradierAuthentication("YOUR_API_KEY");
var client = new TradierClient(httpClient, auth);
```

---

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

For major changes, please [open an issue](https://github.com/blaunius/Tradier.NET/issues) first to discuss what you would like to change.

---

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

## Disclaimer

This SDK is an independent project and is not affiliated with, endorsed by, or connected to Tradier Inc. Use of this software for trading involves financial risk. Always verify order details before execution, especially in production environments. The authors assume no responsibility for financial losses incurred through the use of this software.

---

<div align="center">

**[Tradier API Docs](https://documentation.tradier.com)** · **[Report Bug](https://github.com/blaunius/Tradier.NET/issues)** · **[Request Feature](https://github.com/blaunius/Tradier.NET/issues)**

Made with ❤️ by [Pennyworth Labs](https://github.com/blaunius)

</div>
