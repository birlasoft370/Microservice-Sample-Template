using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Example.Common.Exceptions
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException(string message)
            : base(message)
        {
        }

        public DuplicateRecordException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
