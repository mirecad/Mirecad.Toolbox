using System;
using FluentAssertions;
using Mirecad.Toolbox.Extensions;
using Mirecad.Toolbox.Regex;
using Xunit;

namespace Mirecad.Toolbox.Tests.Regex
{
    public class RegexToolsTests
    {
        [Fact]
        public void GetSingleRegexGroupMatchShouldThrowOnUndefinedGroup()
        {
            var source = "";
            var regex = new System.Text.RegularExpressions.Regex("asdasd");
            var group = "group";

            Action act = () => RegexTools.GetSingleRegexGroupMatch(source, regex, group);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetSingleRegexGroupMatchShouldThrowOnMoreMatches()
        {
            var source = "aaaaaaa";
            var regex = new System.Text.RegularExpressions.Regex("(?<group>a)");
            var group = "group";

            Action act = () => RegexTools.GetSingleRegexGroupMatch(source, regex, group);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetSingleRegexGroupMatchShouldThrowOnNoMatch()
        {
            var source = "aaaaaaa";
            var regex = new System.Text.RegularExpressions.Regex("(?<group>b)");
            var group = "group";

            Action act = () => RegexTools.GetSingleRegexGroupMatch(source, regex, group);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GetSingleRegexGroupCanReturnCorrectMatch()
        {
            var source = "prefix<test>suffix";
            var regex = new System.Text.RegularExpressions.Regex("<(?<group>.*)>");
            var group = "group";

            var match = RegexTools.GetSingleRegexGroupMatch(source, regex, group);

            match.Should().Be("test");
        }

        [Fact]
        public void GetSingleRegexGroupWithStringAsRegexCanReturnCorrectMatch()
        {
            var source = "prefix<test>suffix";
            var regex = "<(?<group>.*)>";
            var group = "group";

            var match = RegexTools.GetSingleRegexGroupMatch(source, regex, group);

            match.Should().Be("test");
        }
    }
}