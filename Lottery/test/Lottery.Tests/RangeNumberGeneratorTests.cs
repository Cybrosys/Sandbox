using Xunit;
using Lottery.Data;
using System.Collections.Generic;
using System.Linq;
using System;

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
            byte minRange = 0;
            byte maxRange = 2;

            // Act
            var number = Generator.Generate(min, max, minRange, maxRange);

            // Assert
            Assert.True(number.Value == 1);
            Assert.True(number.IsOdd);
            Assert.False(number.IsEven);
            Assert.False(number.IsLower);
            Assert.True(number.IsMiddle);
            Assert.False(number.IsUpper);
        }

        [Fact]
        public void Generate_even_middle_numbers()
        {
            // Arrange
            byte min = 2;
            byte max = 2;
            byte minRange = 1;
            byte maxRange = 3;

            // Act
            var number = Generator.Generate(min, max, minRange, maxRange);

            // Assert
            Assert.True(number.Value == 2);
            Assert.False(number.IsOdd);
            Assert.True(number.IsEven);
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

        [Theory]
        [InlineData(0, byte.MaxValue / 2, true)]
        [InlineData(1 + byte.MaxValue / 2, byte.MaxValue, false)]
        public void Generate_lower_then_upper_numbers(int min, int max, bool lowerValues)
        {
            // Arrange
            byte minRange = 0;
            byte maxRange = byte.MaxValue;

            // Act
            var numbers = new List<IRangeNumber>(max);
            for (int i = min; i <= max; ++i)
            {
                numbers.Add(Generator.Generate(i, i, minRange, maxRange));
            }

            // Assert
            if (lowerValues)
                Assert.True(numbers.All(i => i.IsLower));
            else
                Assert.True(numbers.All(i => i.IsUpper));
        }
    }
}
