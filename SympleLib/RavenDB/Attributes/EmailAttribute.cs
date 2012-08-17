using System;
using System.Text.RegularExpressions;

namespace SympleLib.RavenDB.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EmailAttribute : Attribute
    {
        public bool AllowNull { get; set; }

        private const string MatchEmailPattern =
                                                @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
                                                @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
 				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                                 +
                                                @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
 				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                                 +
                                                @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        public bool IsValid(object email)
        {
            if (email is string || email == null)
            {
                if (this.AllowNull == true && string.IsNullOrEmpty((string)email))
                {
                    return true;
                }
                else
                {
                    return Regex.IsMatch((string)email ?? "", MatchEmailPattern);
                }
            }
            else
            {
                throw new Exception("E-Mail Attribute Can Only Be Applied to String Properties");
            }
        }
    }
}
