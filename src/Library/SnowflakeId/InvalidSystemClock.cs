using System;

namespace Library.Snowflake
{
    class InvalidSystemClock : Exception
    {      
        public InvalidSystemClock(string message) : base(message) { }
    }
}