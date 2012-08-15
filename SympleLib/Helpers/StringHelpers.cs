using System;
using System.Collections.Generic;
using System.Reflection;

namespace SympleLib.Helpers
{
    public static class StringHelpers
    {
        
        //-- Parsing
        public static T Parse<T>(this string thingToParse)
        {
            return thingToParse.Parse<T>(default(T));
        }
        
        public static T Parse<T>(this string thingToParse, T defaultValue)
        {
            var retType = typeof (T);
            if(KnownParsers.ContainsKey(retType) != true)
            {
                KnownParsers[retType] = retType.GetMethod("TryParse",
                                                          BindingFlags.Public | BindingFlags.Static, null,
                                                          new[] {typeof (string), retType.MakeByRefType()}, null);
            }
            MethodInfo tParse = KnownParsers[retType];

            if (tParse != null)
            {
                var parameters = new object[] {thingToParse, null};
                var success = (bool) tParse.Invoke(null, parameters);
                if (success)
                {
                    return (T) parameters[1];
                }
            }

            return defaultValue;
        }

        public static T Parse<T>(this string thingToParse, Func<string, T> parser)
        {
            return parser.Invoke(thingToParse);
        }

        private static Dictionary<Type, MethodInfo> _knownParsers;
        public static Dictionary<Type, MethodInfo> KnownParsers
        {
            get { return _knownParsers ?? (_knownParsers = new Dictionary<Type, MethodInfo>()); }
            set { _knownParsers = value; }
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
