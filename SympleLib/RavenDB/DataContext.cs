using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

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

        /// <summary>
        /// Creates a new named Index in the Raven Database 
        /// </summary>
        /// <typeparam name="T">Type Of Object In Index</typeparam>
        /// <param name="indexName">Name Of Index</param>
        public void CreateIndex<T>(string indexName)
        {
            var exists = this._documentStore.DatabaseCommands.GetIndex(indexName) != null;
            if (exists)
            {
                this._documentStore.DatabaseCommands.ResetIndex(indexName);
            }
            else
            {
                this._documentStore.DatabaseCommands.PutIndex(indexName, new IndexDefinitionBuilder<T>
                {
                    Map = documents => documents.Select(entity => new { })
                });
            }
        }

        public void Dispose()
        {
            this._session.Dispose();
            this._documentStore.Dispose();
        }
    }
}
