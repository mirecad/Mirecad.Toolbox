using System.Collections.Generic;
using FluentAssertions;
using Mirecad.Toolbox.Extensions;
using Xunit;

namespace Mirecad.Toolbox.Tests.Extensions
{
    public class EnumerableStringExtensions
    {
        [Fact]
        public void ContainsIgnoreCareCanIgnoreCase()
        {
            var source = new List<string>(){"ONe", "TWO", "three", "FOur"};
            var result = source.ContainsIgnoreCare("foUR");
            result.Should().BeTrue();
        }

    }
}