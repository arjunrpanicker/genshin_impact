using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    [Serializable]
    public class PutHolidayException : Exception
    {
        public PutHolidayException(string message)
            : base(message)
        {
        }

        public PutHolidayException(string message, Exception inner)
            : base(message, inner)
        {
        }

        [ExcludeFromCodeCoverage]
        protected PutHolidayException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
