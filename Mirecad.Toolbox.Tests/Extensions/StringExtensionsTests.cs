using FluentAssertions;
using Mirecad.Toolbox.Extensions;
using Xunit;

namespace Mirecad.Toolbox.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void RemoveDiacriticsCanRemoveLowerCaseDiacritics()
        {
            var input = "ščťžý";
            var result = input.RemoveDiacritics();
            result.Should().Be("sctzy");
        }

        [Fact]
        public void RemoveDiacriticsCanRemoveUpperCaseDiacritics()
        {
            var input = "ŠČŤŽÝ";
            var result = input.RemoveDiacritics();
            result.Should().Be("SCTZY");
        }

        [Fact]
        public void RemoveDiacriticsCanRemoveTabsAndSPaces()
        {
            var source = "Text     with some \t tabs \t\t   and spaces.";
            var result = source.RemoveWhiteSpaces();
            result.Should().Be("Textwithsometabsandspaces.");
        }

        [Fact]
        public void RemoveDiacriticsCanRemoveNewLine()
        {
            var source = "line1\n\nline2";
            var result = source.RemoveWhiteSpaces();
            result.Should().Be("line1line2");
        }

        [Fact]
        public void RemoveSubstringCanRemoveSubstringWithNewLine()
        {
            var source = "line1\n\nline2";
            var result = source.RemoveSubstring("ne1\n\nli");
            result.Should().Be("line2");
        }

        [Fact]
        public void GetSubstringBeforeFirstReturnsWholeStringIfDelimiterNotFound()
        {
            var source = "asdf";
            var result = source.GetSubstringBeforeFirst('G');
            result.Should().Be(source);
        }

        [Fact]
        public void GetSubstringBeforeFirstReturnsCorrectSubstring()
        {
            var source = "asdf";
            var result = source.GetSubstringBeforeFirst('d');
            result.Should().Be("as");
        }

        [Fact]
        public void GetSubstringBeforeFirstReturnsEmptyOnEmptyString()
        {
            var source = string.Empty;
            var result = source.GetSubstringBeforeFirst('g');
            result.Should().Be(string.Empty);
        }
    }
}