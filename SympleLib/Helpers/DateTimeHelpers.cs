using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SympleLib.Helpers
{
    public static class DateTimeHelpers
    {
        public static string ToString(this DateTime? dt, string format)
        {
            return dt != null ? dt.Value.ToString(format) : "";
        }
    }
}
