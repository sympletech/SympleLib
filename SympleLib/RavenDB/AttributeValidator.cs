using System;
using System.Linq;
using SympleLib.RavenDB.Attributes;

namespace SympleLib.RavenDB
{
    public class AttributeValidator
    {
        public static DataObjectOperationResult ValidateDataObject(DataObject dataObj)
        {
            var result = new DataObjectOperationResult();

            CheckRequiredProperties(dataObj, ref result);
            CheckEMailProperties(dataObj, ref result);

            return result;
        }

        private static void CheckRequiredProperties(IDataObject dataObj, ref DataObjectOperationResult result)
        {
            //Get Required Properties
            var requiredProperties = dataObj.GetType()
                                            .GetProperties()
                                            .Where(x => Attribute.IsDefined(x, typeof(RequiredValueAttribute)));

            //Check to Ensure Required Properties are not Null or empty
            foreach (var reqProp in requiredProperties)
            {
                var prop = dataObj.GetType().GetProperty(reqProp.Name);
                var propValue = prop.GetValue(dataObj, null);
                bool isNull = false;

                switch (prop.PropertyType.Name.ToLower())
                {
                    case "string":
                        isNull = string.IsNullOrEmpty((string)propValue);
                        break;
                    default:
                        isNull = propValue == null;
                        break;
                }

                if (isNull)
                {
                    RequiredValueAttribute reqAttrib = (RequiredValueAttribute)(prop.GetCustomAttributes(typeof(RequiredValueAttribute), false)[0]);
                    //var reqAttrib = prop.GetCustomAttribute<RequiredValueAttribute>();
                    result.Message = "A Required Value is Missing";
                    result.ErrorMessages.Add(reqProp.Name, reqAttrib.ErrorMessage);
                    result.Success = false;
                }
            }
        }

        private static void CheckEMailProperties(IDataObject dataObj, ref DataObjectOperationResult result)
        {
            //Get E-Mail Properties
            var emailProperties = dataObj.GetType()
                                         .GetProperties()
                                         .Where(x => Attribute.IsDefined(x, typeof(EmailAttribute)));

            //Check to Ensure Required Properties are Valid E-Mail Addresses
            foreach (var emlProp in emailProperties)
            {
                var prop = dataObj.GetType().GetProperty(emlProp.Name);
                var propValue = prop.GetValue(dataObj, null);
                
                //var emlAttrib = prop.GetCustomAttribute<EmailAttribute>();
                EmailAttribute emlAttrib = (EmailAttribute)(prop.GetCustomAttributes(typeof(EmailAttribute), false)[0]);
                
                if (emlAttrib.IsValid(propValue) != true)
                {
                    result.Message = "An Error Occured While Validating Input";
                    result.ErrorMessages.Add(emlProp.Name, "Not A Valid E-Mail Address");
                    result.Success = false;
                }
            }
        }
    }
}
