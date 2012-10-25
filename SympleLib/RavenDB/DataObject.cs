using Raven.Imports.Newtonsoft.Json;
using System;

namespace SympleLib.RavenDB
{
    public class DataObject: IDataObject
    {
        //-- Properties

        public string Id { get; set; }

        //-- ReLoad

        public void Reload()
        {
            DataContext.Db.Session.Advanced.Refresh(this);
        }

        //-- CRUD

        public DataObjectOperationResult Save()
        {
            var result = AttributeValidator.ValidateDataObject(this);
            if (result.Success == true)
            {
                DataContext.Db.Session.Store(this);
                return this.Commit();
            }
            return result;
        }

        public DataObjectOperationResult Delete()
        {
            DataContext.Db.Session.Delete(this);
            return this.Commit();
        }

        private DataObjectOperationResult Commit()
        {
            var result = new DataObjectOperationResult();
            try
            {
                DataContext.Db.Session.Store(this);
                DataContext.Db.Session.SaveChanges();

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
