using System;

namespace SympleLib.RavenDB.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredValueAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
    }
}
