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
    }
}