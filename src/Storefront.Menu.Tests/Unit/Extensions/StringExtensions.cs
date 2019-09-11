using Storefront.Menu.API.Extensions;
using Xunit;

namespace Storefront.Menu.Tests.Unit.Extensions
{
    public sealed class StringExtensions
    {
        [Fact]
        public void ShouldReturnWords()
        {
            var words = "Storefront Community Menu API".Words();

            Assert.Equal(4, words.Length);
            Assert.Equal("Storefront", words[0]);
            Assert.Equal("Community", words[1]);
            Assert.Equal("Menu", words[2]);
            Assert.Equal("API", words[3]);
        }
    }
}
