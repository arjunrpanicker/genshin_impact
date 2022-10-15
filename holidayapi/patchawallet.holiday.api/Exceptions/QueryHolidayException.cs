using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    [Serializable]
    public class QueryHolidayException : Exception
    {
        public QueryHolidayException(string message)
           : base(message)
        {
        }

        public QueryHolidayException(string message, Exception inner)
            : base(message, inner)
        {
        }

        [ExcludeFromCodeCoverage]
        protected QueryHolidayException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }

    }
}
