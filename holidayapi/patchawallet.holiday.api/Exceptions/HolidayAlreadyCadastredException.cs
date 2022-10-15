using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace patchawallet.holiday.api
{
    [Serializable]
    public class HolidayAlreadyCadastredException : Exception
    {
        public HolidayAlreadyCadastredException(string message)
            : base(message)
        {
        }

        [ExcludeFromCodeCoverage]
        protected HolidayAlreadyCadastredException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
