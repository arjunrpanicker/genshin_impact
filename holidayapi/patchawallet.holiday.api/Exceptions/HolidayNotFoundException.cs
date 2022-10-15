using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    [Serializable]
    public class HolidayNotFoundException : Exception
    {
        public HolidayNotFoundException(string message)
            : base(message)
        {
        }

        [ExcludeFromCodeCoverage]
        protected HolidayNotFoundException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
