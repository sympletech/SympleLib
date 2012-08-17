using System.Collections.Generic;
using System.Web.Mvc;

namespace SympleLib.RavenDB
{
    public class DataObjectOperationResult
    {
        public DataObjectOperationResult()
        {
            Success = true;
            ErrorMessages = new Dictionary<string, string>();
        }

        public bool Success { get; set; }
        public string Message { get; set; }

        public Dictionary<string, string> ErrorMessages { get; set; }

        public void PassErrorsToMvcModelState(ModelStateDictionary modelState)
        {
            foreach (var eMsg in ErrorMessages)
            {
                modelState.AddModelError(eMsg.Key, eMsg.Value);
            }
        }
    }
}
