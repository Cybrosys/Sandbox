using Xunit;
using Lottery.Data;

namespace Lottery.Tests
{
    public class RangeNumberGeneratorTests
    {
        private RangeNumberGenerator _generator;
        private RangeNumberGenerator Generator => _generator ?? (_generator = new RangeNumberGenerator(new Data.Random()));

        [Fact]
        public void Generate_odd_middle_number() 
        {
            // Arrange
            byte min = 1;
            byte max = 1;

            // Act
            var number = Generator.Generate(min, max);

            // Assert
            Assert.True(number.Value == 1);
            Assert.True(number.IsOdd);
            Assert.False(number.IsEven);
            Assert.False(number.IsLower);
            Assert.True(number.IsMiddle);
            Assert.False(number.IsUpper);
        }

        [Fact]
        public void Generate_odd_lower_number()
        {
            // Arrange
            byte min = 1;
            byte max = 1;
            byte minRange = 1;
            byte maxRange = byte.MaxValue;

            // Act
            var number = Generator.Generate(min, max, minRange, maxRange);

            // Assert
            Assert.True(number.Value == 1);
            Assert.True(number.IsOdd);
            Assert.False(number.IsEven);
            Assert.True(number.IsLower);
            Assert.False(number.IsMiddle);
            Assert.False(number.IsUpper);
        }

        [Fact]
        public void Generate_odd_upper_number()
        {
            // Arrange
            byte min = byte.MaxValue;
            byte max = byte.MaxValue;
            byte minRange = 1;
            byte maxRange = byte.MaxValue;

            // Act
            var number = Generator.Generate(min, max, minRange, maxRange);

            // Assert
            Assert.True(number.Value == byte.MaxValue);
            Assert.True(number.IsOdd);
            Assert.False(number.IsEven);
            Assert.False(number.IsLower);
            Assert.False(number.IsMiddle);
            Assert.True(number.IsUpper);
        }
    }
}
