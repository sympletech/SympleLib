using System;
using System.Linq;
using System.Linq.Expressions;
using Raven.Imports.Newtonsoft.Json;

namespace SympleLib.RavenDB
{
    public class DataObject<T> : IDataObject where T : IDataObject
    {
        //-- Properties

        [JsonIgnore]
        public IDataContext Db { get; set; }

        public string Id { get; set; }

        //-- ReLoad

        public void Reload()
        {
            Db.Session.Advanced.Refresh(this);
        }

        //-- CRUD

        public DataObjectOperationResult Save()
        {
            var result = new DataObjectOperationResult();
            try
            {
                AttributeValidator.ValidateDataObject(this, ref result);
                if (result.Success == true)
                {
                    Db.Session.Store(this);
                    Db.Session.SaveChanges();

                    result.ObjectID = this.Id;
                    result.Message = "Database Update Completed Successfully";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public DataObjectOperationResult Delete()
        {
            try
            {
                Db.Session.Delete(this);
                Db.Session.SaveChanges();

                var results = new DataObjectOperationResult
                {
                    Message = "Object Has Been Deleted",
                    ObjectID = this.Id,
                    Success = true
                };

                this.Id = null;

                return results;
            }
            catch (Exception Ex)
            {
                return new DataObjectOperationResult { 
                    Success = false,
                    Message = Ex.Message
                };
            }

        }
    }
}
