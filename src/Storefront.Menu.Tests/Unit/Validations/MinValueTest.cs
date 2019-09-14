using System;
using Storefront.Menu.API.Models.TransferModel.Validations;
using Xunit;

namespace Storefront.Menu.Tests.Unit.Validations
{
    public sealed class MinValueTest
    {
        [Fact]
        public void ShouldBeValid()
        {
            Assert.True(new MinValue(10.5).IsValid(null));
            Assert.True(new MinValue(10.5).IsValid(10.5M));
            Assert.True(new MinValue(10.5).IsValid(10.5));
            Assert.True(new MinValue(10.5).IsValid(10.6));
            Assert.True(new MinValue(10.5).IsValid(11));
        }

        [Fact]
        public void ShouldBeInvalid()
        {
            Assert.False(new MinValue(10.5).IsValid(10.4));
        }

        [Fact]
        public void ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new MinValue(5).IsValid("Not a number"));
        }
    }
}
