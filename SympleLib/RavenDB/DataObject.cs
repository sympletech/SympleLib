using System;
using System.Linq;
using System.Linq.Expressions;
using Raven.Imports.Newtonsoft.Json;

namespace SympleLib.RavenDB
{
    public class DataObject<T> : IDataObject where T : IDataObject
    {
        //-- Constructors
        [JsonIgnore]
        public IDataContext Db { get; set; }
        public DataObject(IDataContext db)
        {
            this.Db = db;
        }

        //-- Properties

        public string Id { get; set; }

        //-- Lookups

        /// <summary>
        /// Get A Single Object By ID
        /// </summary>
        public static T Get(IDataContext db, string id)
        {
            var result = db.Session
                           .Query<T>()
                           .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                           .FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                db.Attach(result);
            }

            return result;
        }

        /// <summary>
        /// Get A Single Object By Query
        /// </summary>
        public static T Get(IDataContext db, Expression<Func<T, bool>> predicate)
        {
            var result = db.Session
                           .Query<T>()
                           .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                           .FirstOrDefault(predicate);

            if (result != null)
            {
                db.Attach(result);
            }

            return result;
        }

        /// <summary>
        /// Returns Entire Collection of Objects
        /// </summary>
        public static IQueryable<T> GetAll(IDataContext db)
        {
            return db.Session.Query<T>();
        }

        /// <summary>
        /// Performs a Query agianst the collection
        /// </summary>
        public static IQueryable<T> Query(IDataContext db, Expression<Func<T, bool>> predicate)
        {
            return db.Session.Query<T>().Where(predicate);
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

        public void Delete()
        {
            Db.Session.Delete(this);
            Db.Session.SaveChanges();
            this.Id = null;
        }
    }
}
