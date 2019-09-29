using StorefrontCommunity.Menu.API.Extensions;
using Xunit;

namespace StorefrontCommunity.Menu.Tests.Unit.Extensions
{
    public sealed class StringExtensions
    {
        [Fact]
        public void ShouldReturnWords()
        {
            var words = "StorefrontCommunity Community Menu API".Words();

            Assert.Equal(4, words.Length);
            Assert.Equal("StorefrontCommunity", words[0]);
            Assert.Equal("Community", words[1]);
            Assert.Equal("Menu", words[2]);
            Assert.Equal("API", words[3]);
        }
    }
}
