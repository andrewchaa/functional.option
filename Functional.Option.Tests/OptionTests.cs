using System.Threading.Tasks;
using Xunit;

namespace Functional.Option.Tests
{
    public class OptionTests
    {
        private async Task<string> GetNameAsync()
            => "My name is Trinity";

        [Fact]
        public void Should_compare_cointained_value()
        {
            Assert.Equal(F.Some(20), F.Some(20));
        }

        [Fact]
        public void Should_convert_inner_value()
        {
            var optionString = F.Some("true");
            var optionBool = optionString.Map(bool.Parse);

            Assert.Equal(F.Some(true), optionBool);
        }
    }
}
