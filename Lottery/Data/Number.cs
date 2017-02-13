namespace Lottery
{
    public class Number : INumber
    {
        public byte Value { get; set; }
        public bool IsOdd { get; set; }
        public bool IsEven { get; set; }
        public bool IsUpper { get; set; }
        public bool IsLower { get; set; }
    }
}