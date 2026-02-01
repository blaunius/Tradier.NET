using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tradier.Request;

namespace Tradier.Tests.Request
{
    [TestClass]
    public abstract class RequestTestsBase
    {
        protected void AssertQueryString(ITradierRequest request, string expected, string? message = null)
        {
            var actual = request.ToQueryString();
            Assert.AreEqual(expected, actual, message ?? $"Query string mismatch. Expected: '{expected}', Actual: '{actual}'");
        }

        protected void AssertQueryStringContains(ITradierRequest request, string expectedParameter, string? message = null)
        {
            var queryString = request.ToQueryString();
            Assert.IsTrue(queryString.Contains(expectedParameter), 
                message ?? $"Query string '{queryString}' does not contain expected parameter '{expectedParameter}'");
        }

        protected void AssertQueryStringNotContains(ITradierRequest request, string unexpectedParameter, string? message = null)
        {
            var queryString = request.ToQueryString();
            Assert.IsFalse(queryString.Contains(unexpectedParameter), 
                message ?? $"Query string '{queryString}' should not contain parameter '{unexpectedParameter}'");
        }

        protected void AssertEmptyQueryString(ITradierRequest request, string? message = null)
        {
            var queryString = request.ToQueryString();
            Assert.IsTrue(string.IsNullOrEmpty(queryString), 
                message ?? $"Expected empty query string but got: '{queryString}'");
        }
    }
}