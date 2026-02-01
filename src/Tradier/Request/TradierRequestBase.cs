using System.Text;
using System.Web;

namespace Tradier.Request
{
    public abstract class TradierRequestBase : ITradierRequest
    {
        private readonly Dictionary<string, string> _parameters = new();

        protected void AddParameter(string name, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _parameters[name] = value;
        }

        protected void AddParameter(string name, DateTime? value, string format = "yyyy-MM-dd")
        {
            if (value.HasValue)
                _parameters[name] = value.Value.ToString(format);
        }

        protected void AddParameter(string name, bool value, string trueValue = "true", string falseValue = "false")
        {
            _parameters[name] = value ? trueValue : falseValue;
        }

        protected void AddParameter<T>(string name, T? value) where T : struct, Enum
        {
            if (value.HasValue)
                _parameters[name] = value.Value.ToString().ToLowerInvariant();
        }

        protected virtual void BuildParameters()
        {
        }

        public virtual string ToQueryString()
        {
            _parameters.Clear();
            BuildParameters();
            
            if (_parameters.Count == 0)
                return string.Empty;

            return string.Join("&", _parameters.Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"));
        }
    }
}