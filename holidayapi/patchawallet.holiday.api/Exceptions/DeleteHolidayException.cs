using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    [Serializable]
    public class DeleteHolidayException : Exception
    {
        public DeleteHolidayException(string message)
            : base(message)
        {
        }

        public DeleteHolidayException(string message, Exception inner)
            : base(message, inner)
        {
        }

        [ExcludeFromCodeCoverage]
        protected DeleteHolidayException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
