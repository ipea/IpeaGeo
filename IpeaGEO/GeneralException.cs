using System;
using System.Runtime.Serialization;

namespace IpeaGeo
{
    /// <summary>
    /// General exception class.
    /// <see cref="Exception"/>
    /// </summary>
    [Serializable]
    public class GeneralException : Exception
    {
        public GeneralException() : base() { }

        protected GeneralException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public GeneralException(string message) : base(message) { }

        public GeneralException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
