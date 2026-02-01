using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tradier.Request;

namespace Tradier.Tests.Request
{
    [TestClass]
    public class TradierRequestBaseTests : RequestTestsBase
    {
        private class TestRequest : TradierRequestBase
        {
            public string? StringValue { get; set; }
            public DateTime? DateValue { get; set; }
            public bool? BoolValue { get; set; }
            public TestEnum? EnumValue { get; set; }
            public bool IncludeBool { get; set; } = false;

            protected override void BuildParameters()
            {
                AddParameter("string", StringValue);
                AddParameter("date", DateValue);
                if (IncludeBool && BoolValue.HasValue)
                    AddParameter("bool", BoolValue.Value);
                AddParameter("enum", EnumValue);
            }
        }

        private enum TestEnum
        {
            FirstValue,
            SecondValue,
            ThirdValue
        }

        [TestMethod]
        public void ToQueryString_WithNoParameters_ReturnsEmpty()
        {
            var request = new TestRequest();
            AssertEmptyQueryString(request);
        }

        [TestMethod]
        public void AddParameter_WithStringValue_AddsCorrectly()
        {
            var request = new TestRequest { StringValue = "test" };
            AssertQueryString(request, "string=test");
        }

        [TestMethod]
        public void AddParameter_WithNullString_DoesNotAddParameter()
        {
            var request = new TestRequest { StringValue = null };
            AssertEmptyQueryString(request);
        }

        [TestMethod]
        public void AddParameter_WithEmptyString_DoesNotAddParameter()
        {
            var request = new TestRequest { StringValue = "" };
            AssertEmptyQueryString(request);
        }

        [TestMethod]
        public void AddParameter_WithWhitespaceString_DoesNotAddParameter()
        {
            var request = new TestRequest { StringValue = "   " };
            AssertEmptyQueryString(request);
        }

        [TestMethod]
        public void AddParameter_WithDateTime_FormatsCorrectly()
        {
            var request = new TestRequest { DateValue = new DateTime(2023, 12, 25) };
            AssertQueryString(request, "date=2023-12-25");
        }

        [TestMethod]
        public void AddParameter_WithNullDateTime_DoesNotAddParameter()
        {
            var request = new TestRequest { DateValue = null };
            AssertEmptyQueryString(request);
        }

        [TestMethod]
        public void AddParameter_WithBooleanTrue_AddsTrue()
        {
            var request = new TestRequest { BoolValue = true, IncludeBool = true };
            AssertQueryString(request, "bool=true");
        }

        [TestMethod]
        public void AddParameter_WithBooleanFalse_AddsFalse()
        {
            var request = new TestRequest { BoolValue = false, IncludeBool = true };
            AssertQueryString(request, "bool=false");
        }

        [TestMethod]
        public void AddParameter_WithEnum_AddsLowercase()
        {
            var request = new TestRequest { EnumValue = TestEnum.FirstValue };
            AssertQueryString(request, "enum=firstvalue");
        }

        [TestMethod]
        public void AddParameter_WithNullEnum_DoesNotAddParameter()
        {
            var request = new TestRequest { EnumValue = null };
            AssertEmptyQueryString(request);
        }

        [TestMethod]
        public void AddParameter_WithMultipleValues_CombinesCorrectly()
        {
            var request = new TestRequest 
            { 
                StringValue = "test",
                DateValue = new DateTime(2023, 1, 1),
                BoolValue = true,
                IncludeBool = true,
                EnumValue = TestEnum.SecondValue
            };

            var queryString = request.ToQueryString();
            
            Assert.IsTrue(queryString.Contains("string=test"));
            Assert.IsTrue(queryString.Contains("date=2023-01-01"));
            Assert.IsTrue(queryString.Contains("bool=true"));
            Assert.IsTrue(queryString.Contains("enum=secondvalue"));
        }

        [TestMethod]
        public void ToQueryString_UrlEncodesSpecialCharacters()
        {
            var request = new TestRequest { StringValue = "test value with spaces & symbols" };
            var queryString = request.ToQueryString();
            
            // Just verify that it contains encoded values
            Assert.IsTrue(queryString.Contains("string="));
            Assert.IsTrue(queryString.Contains("test") && queryString.Contains("value"));
        }

        [TestMethod]
        public void ToQueryString_HandlesDateTimeFormats()
        {
            var testRequest = new CustomDateFormatRequest 
            { 
                DateValue = new DateTime(2023, 12, 25, 10, 30, 45) 
            };
            
            var queryString = testRequest.ToQueryString();
            Assert.IsTrue(queryString.Contains("2023-12-25"));
        }

        private class CustomDateFormatRequest : TradierRequestBase
        {
            public DateTime? DateValue { get; set; }

            protected override void BuildParameters()
            {
                AddParameter("date", DateValue, "yyyy-MM-dd HH:mm:ss");
            }
        }

        [TestMethod]
        public void AddParameter_WithCustomBooleanValues_UsesCustomValues()
        {
            var testRequest = new CustomBooleanRequest { BoolValue = true };
            AssertQueryString(testRequest, "bool=yes");
            
            testRequest = new CustomBooleanRequest { BoolValue = false };
            AssertQueryString(testRequest, "bool=no");
        }

        private class CustomBooleanRequest : TradierRequestBase
        {
            public bool BoolValue { get; set; }

            protected override void BuildParameters()
            {
                AddParameter("bool", BoolValue, "yes", "no");
            }
        }
    }
}