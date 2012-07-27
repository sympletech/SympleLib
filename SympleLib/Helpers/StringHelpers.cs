using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SympleLib.Helpers
{
    public static class StringHelpers
    {
        //-- Parsing

        public static T Parse<T>(this string thingToParse)
        {
            var retType = typeof (T);
            var tParse = retType.GetMethod("TryParse",
                                           BindingFlags.Public | BindingFlags.Static, null,
                                           new[] {typeof (string), retType.MakeByRefType()}, null);

            if (tParse != null)
            {
                var parameters = new object[] {thingToParse, null};
                var success = (bool) tParse.Invoke(null, parameters);
                if (success)
                {
                    return (T) parameters[1];
                }
            }

            return default(T);
        }

        public static T Parse<T>(this string thingToParse, Func<string, T> parser)
        {
            return parser.Invoke(thingToParse);
        }
    
    
        //-- NUll Check 

        public static bool IsNotEmpty(this string stringToCheck)
        {
            return string.IsNullOrEmpty(stringToCheck) != true;
        }

        public static bool IsEmpty(this string stringToCheck)
        {
            return string.IsNullOrEmpty(stringToCheck);
        }

        
    }
}
