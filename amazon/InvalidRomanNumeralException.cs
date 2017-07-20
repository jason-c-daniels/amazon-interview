using System;

namespace amazon
{
    public class InvalidRomanNumeralException : Exception
    {
        public InvalidRomanNumeralException() { }
        public InvalidRomanNumeralException(string message) : base(message) { }
        public InvalidRomanNumeralException(string message, Exception inner) : base(message, inner) { }
    }
}
