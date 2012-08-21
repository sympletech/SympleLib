using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SympleLib.Helpers
{
    public static class ObjectHelpers
    {
        public static T UpCast<T>(this object baseObject, params string[] excludeProps)
        {
            var sourceType = baseObject.GetType();
            Type derivedType = typeof(T);
            var result = Activator.CreateInstance<T>();

            foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
            {
                //Skip if in the exclude list
                if (excludeProps.Contains(sourceProperty.Name) != true && NativeTypes.Contains(sourceProperty.PropertyType))
                {
                    var sourcePropertyValue = sourceProperty.GetValue(baseObject, null);
                    if (sourcePropertyValue != null)
                    {
                        var destinationProperty = derivedType.GetProperty(sourceProperty.Name);
                        if (destinationProperty != null)
                        {
                            try { destinationProperty.SetValue(result, sourcePropertyValue, null); }
                            catch { }
                        }
                    }
                }
            }

            return result;
        }

        public static List<Type> NativeTypes
        {
            get
            {
                var approvedTypes = new List<Type>();

                approvedTypes.Add(typeof(int));
                approvedTypes.Add(typeof(Int32));
                approvedTypes.Add(typeof(Int64));
                approvedTypes.Add(typeof(string));
                approvedTypes.Add(typeof(DateTime));
                approvedTypes.Add(typeof(double));
                approvedTypes.Add(typeof(decimal));
                approvedTypes.Add(typeof(float));
                approvedTypes.Add(typeof(List<>));
                approvedTypes.Add(typeof(bool));
                approvedTypes.Add(typeof(Boolean));

                approvedTypes.Add(typeof(int?));
                approvedTypes.Add(typeof(Int32?));
                approvedTypes.Add(typeof(Int64?));
                approvedTypes.Add(typeof(DateTime?));
                approvedTypes.Add(typeof(double?));
                approvedTypes.Add(typeof(decimal?));
                approvedTypes.Add(typeof(float?));
                approvedTypes.Add(typeof(bool?));
                approvedTypes.Add(typeof(Boolean?));

                return approvedTypes;
            }
        }


        public static T MapTo<T>(this object baseObject)
        {
            Mapper.CreateMap(baseObject.GetType(), typeof(T));
            return Mapper.Map<T>(baseObject);
        }
    }
}
