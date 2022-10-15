using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    [Serializable]
    public class PostHolidayException : Exception
    {
        public PostHolidayException(string message)
            : base(message)
        {
        }

        public PostHolidayException(string message, Exception inner)
            : base(message, inner)
        {
        }

        [ExcludeFromCodeCoverage]
        protected PostHolidayException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
