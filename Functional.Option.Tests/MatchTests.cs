using System;
using Xunit;

namespace Functional.Option.Tests
{
    public class MatchTests
    {
        private string Greet(Option<string> name)
            => name.Match(
                Some: n => $"hello, {n}",
                None: () => "sorry, who?");

        private Option<string> GetName(bool real)
            => real
                ? F.Some("Name")
                : F.None;

        [Fact]
        public void Match_should_call_appropriate_func()
        {
            Assert.Equal("hello, John", Greet(F.Some("John")));
            Assert.Equal("sorry, who?", Greet(F.None));
        }

        [Fact]
        public void Match_should_handle_action()
        {
            var result = GetName(false);
            result.Match(
                None: () => Console.Write("Nothing"),
                Some: Console.Write
            );

            Assert.Equal(F.None, GetName(false));
        }

    }
}