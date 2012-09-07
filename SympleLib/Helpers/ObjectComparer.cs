using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SympleLib.Helpers
{
    public class ObjectComparer
    {
        /// <summary>
        /// Compares two objects of the same type and determines if they are equal by checking each property.
        /// Returns a list of changed properties with base and changed value
        /// </summary>
        public static ComparerResult Compare<T>(T objA, T objB, params string[] excludeFields)
        {
            var results = new ComparerResult { 
                AreEqual = true,
                ChangedProperties = new List<ComparerResultEntry>()
            };

            var objType = typeof(T);
            
            foreach (PropertyInfo objProp in objType.GetProperties())
            {
                if (excludeFields.Contains(objProp.Name) != true && ObjectHelpers.NativeTypes.Contains(objProp.PropertyType))
                {
                    var objAPropertyValue = objProp.GetValue(objA, null);
                    var objBPropertyValue = objProp.GetValue(objB, null);
                    
                    //If properties are diffrent - mark as diffrent and add details to results
                    if (objAPropertyValue != objBPropertyValue)
                    {
                        results.AreEqual = false;
                        results.ChangedProperties.Add(new ComparerResultEntry {
                            PropertyName = objProp.Name,
                            PropertyType = objProp.PropertyType,
                            ObjAValue = objAPropertyValue,
                            ObjBValue = objBPropertyValue
                        });
                    }
                }
            }

            return results;
        }



        //-- Return Types

        public class ComparerResult
        {
            public bool AreEqual { get; set; }
            public List<ComparerResultEntry> ChangedProperties { get; set; }
        }

        public class ComparerResultEntry
        {
            public string PropertyName { get; set; }
            public Type PropertyType { get; set; }
            public object ObjAValue { get; set; }
            public object ObjBValue { get; set; }
        }

    }


}
