using Raven.Client;
using Raven.Client.Document;
using System;

namespace SympleLib.RavenDB
{
    public class DataContext : IDataContext, IDisposable 
    {
        private IDocumentStore _documentStore;
        public IDocumentStore DocumentStore
        {
            get
            {
                return _documentStore;
            }
            set
            {
                _documentStore = value;
            }
        }
        
        private IDocumentSession _session;
        public IDocumentSession Session
        {
            get
            {
                return this._session;
            }
        }

        public DataContext(string connectionStringName, int maxMaxNumberOfRequestsPerSession = 30)
        {
            this._documentStore = new DocumentStore
            {
                ConnectionStringName = connectionStringName,
                Conventions = new DocumentConvention
                {
                    MaxNumberOfRequestsPerSession = maxMaxNumberOfRequestsPerSession
                }
            }.Initialize();

            this._session = this._documentStore.OpenSession();
            
            //Prevents Stale Records - Makes Raven slower but more trust worthy
            this._session.Advanced.AllowNonAuthoritativeInformation = false;
        }

        private static DataContext _db{get;set;}
        
        /// <summary>
        /// Singleton Instance of DataContext -- Assumes Connection String named "RavenDB"
        /// </summary>
        public static DataContext DB
        {
            get
            {
                if (_db == null)
                {
                    _db = new DataContext("RavenDB", 30);
                }
                return _db;
            }
        }

        public void Dispose()
        {
            this._session.Dispose();
            this._documentStore.Dispose();
        }
    }
}
