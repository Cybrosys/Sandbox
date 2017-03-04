namespace Lottery
{
    public class RangeNumber : IRangeNumber
    {
        public byte Value { get; set; }
        public bool IsOdd { get; set; }
        public bool IsEven { get; set; }
        public bool IsUpper { get; set; }
        public bool IsMiddle { get; set; }
        public bool IsLower { get; set; }
    }
}