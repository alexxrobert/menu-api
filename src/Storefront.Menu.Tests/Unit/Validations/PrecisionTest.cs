using Storefront.Menu.API.Models.TransferModel.Validations;
using Xunit;

namespace Storefront.Menu.Tests.Unit.Validations
{
    public sealed class PrecisionTest
    {
        [Fact]
        public void ShouldBeValid()
        {
            Assert.True(new Precision(1).IsValid(null));
            Assert.True(new Precision(1).IsValid(9));
            Assert.True(new Precision(5, 2).IsValid(123.12));
            Assert.True(new Precision(5, 2).IsValid(123.1));
            Assert.True(new Precision(5, 2).IsValid(123));
        }

        [Fact]
        public void ShouldBeInvalid()
        {
            Assert.False(new Precision(5, 2).IsValid(1234.12));
            Assert.False(new Precision(5, 2).IsValid(123.123));
            Assert.False(new Precision(5, 2).IsValid("Not a number"));
        }
    }
}
