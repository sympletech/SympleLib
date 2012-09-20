using Raven.Imports.Newtonsoft.Json;
using System;

namespace SympleLib.RavenDB
{
    public class DataObject: IDataObject
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
            var result = AttributeValidator.ValidateDataObject(this);
            if (result.Success == true)
            {
                Db.Session.Store(this);
                return this.Commit();
            }
            return result;
        }

        public DataObjectOperationResult Delete()
        {
            Db.Session.Delete(this);
            return this.Commit();
        }

        private DataObjectOperationResult Commit()
        {
            var result = new DataObjectOperationResult();
            try
            {
                Db.Session.Store(this);
                Db.Session.SaveChanges();

                result.ObjectID = this.Id;
                result.Success = true;
                result.Message = "Database Update Completed Successfully";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
