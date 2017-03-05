using Xunit;
using Lottery.Data;

namespace Lottery.Tests
{
    public class RangeNumberGeneratorTests
    {
        [Fact]
        public void Generate_only_middle_range_numbers() 
        {
            // Arrange
            byte min = 1;
            byte max = 1;
            var generator = new RangeNumberGenerator(new Data.Random());

            // Act
            var number = generator.Generate(min, max);

            // Assert
            Assert.True(number.Value == 1);
            Assert.True(number.IsOdd);
            Assert.True(number.IsMiddle);
        }
    }
}
