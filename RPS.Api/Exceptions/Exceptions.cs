using System;

namespace RPS.Api.Exceptions
{
    [Serializable]
    public class MyException : Exception
    {
        public MyException() { }
        public MyException(string message) : base(message) { }
        public MyException(string message, Exception inner) : base(message, inner) { }
        protected MyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidNumberOfGamesException : Exception
    {
        public InvalidNumberOfGamesException() { }
        public InvalidNumberOfGamesException(string message) : base(message) { }
        public InvalidNumberOfGamesException(string message, Exception inner) : base(message, inner) { }
        protected InvalidNumberOfGamesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidNumberOfDynamitesException : Exception
    {
        public InvalidNumberOfDynamitesException() { }
        public InvalidNumberOfDynamitesException(string message) : base(message) { }
        public InvalidNumberOfDynamitesException(string message, Exception inner) : base(message, inner) { }
        protected InvalidNumberOfDynamitesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
