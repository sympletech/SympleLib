using Raven.Client;
using Raven.Client.Document;
using System;
using Raven.Client.Embedded;

namespace SympleLib.RavenDB
{
    public class DataContext : IDataContext, IDisposable
    {
        //-- Static Constructors (To enforce singleton)

        private static string _connectionStringName;
        public static string ConnectionStringName
        {
            get
            {
                if (_connectionStringName == null)
                {
                    _connectionStringName = "RavenDB";
                }
                return _connectionStringName;
            }
            set
            {
                _connectionStringName = value;
            }
        }

        private static bool _runInMemory;
        public static bool RunInMemory
        {
            get
            {
                if (_runInMemory == null)
                {
                    _runInMemory = false;
                }
                return _runInMemory;
            }
            set { _runInMemory = value; }
        }

        private static DataContext _db { get; set; }

        /// <summary>
        /// Singleton Instance of DataContext -- Assumes Connection String named "RavenDB"
        /// </summary>
        public static DataContext Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new DataContext(ConnectionStringName, 30);
                }
                return _db;
            }
        }


        //-- Private constructors

        public IDocumentStore DocumentStore;

        private IDocumentSession _session;
        public IDocumentSession Session
        {
            get
            {
                return this._session;
            }
        }

        private DataContext(string connectionStringName, int maxMaxNumberOfRequestsPerSession = 30)
        {
            if (RunInMemory == true)
            {
                this.DocumentStore = new EmbeddableDocumentStore { RunInMemory = true };
            }
            else
            {
                this.DocumentStore = new DocumentStore
                {
                    ConnectionStringName = connectionStringName,
                    Conventions = new DocumentConvention
                    {
                        MaxNumberOfRequestsPerSession = maxMaxNumberOfRequestsPerSession
                    }
                };
            }
            this.DocumentStore.Initialize();

            this._session = this.DocumentStore.OpenSession();

            //Prevents Stale Records - Makes Raven slower but more trust worthy
            this._session.Advanced.AllowNonAuthoritativeInformation = false;
        }

        public void Dispose()
        {
            this._session.Dispose();
            this.DocumentStore.Dispose();
        }
    }
}
