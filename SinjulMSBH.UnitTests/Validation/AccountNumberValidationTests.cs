using System;

using SinjulMSBH.WebUI.Validation;

using Xunit;

namespace SinjulMSBH.UnitTests.Validation
{
    public class AccountNumberValidationTests
    {
        public AccountNumberValidationTests() => AccountNumberValidation = new AccountNumberValidation();
        private readonly AccountNumberValidation AccountNumberValidation;

        [Fact]
        public void IsValid_ValidAccountNumber_ReturnsTrue() =>
            Assert.True(AccountNumberValidation.IsValid("123-4543234576-23"));

        [Theory]
        [InlineData("1234-3454565676-23")]
        [InlineData("12-3454565676-23")]
        public void IsValid_AccountNumberFirstPartWrong_ReturnsFalse(string accountNumber) =>
            Assert.False(AccountNumberValidation.IsValid(accountNumber));

        [Theory]
        [InlineData("123-345456567-23")]
        [InlineData("123-345456567633-23")]
        public void IsValid_AccountNumberMiddlePartWrong_ReturnsFalse(string accNumber) =>
            Assert.False(AccountNumberValidation.IsValid(accNumber));

        [Theory]
        [InlineData("123-3454565673-2")]
        [InlineData("123-3454565676-233")]
        public void IsValid_AccountNumberLastPartWrong_ReturnsFalse(string accNumber) =>
            Assert.False(AccountNumberValidation.IsValid(accNumber));

        [Theory]
        [InlineData("123-345456567633=23")]
        [InlineData("123+345456567633-23")]
        [InlineData("123+345456567633=23")]
        public void IsValid_InvalidDelimiters_ThrowsArgumentException(string accNumber) =>
            Assert.Throws<ArgumentException>(() => AccountNumberValidation.IsValid(accNumber));
    }
}
