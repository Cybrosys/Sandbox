using System;
using Lottery.Data;

namespace Lottery
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var generator = new RangeNumberGenerator(new Data.Random());
            var number = generator.Generate(1, 3);
        }
    }
}
