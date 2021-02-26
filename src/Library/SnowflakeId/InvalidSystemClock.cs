using System;

namespace Microservice.Library.Snowflake
{
    class InvalidSystemClock : Exception
    {      
        public InvalidSystemClock(string message) : base(message) { }
    }
}