using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace SympleLib.RavenDB
{
    public class DataContext : IDataContext
    {
        private readonly IDocumentStore _documentStore;
        private IDocumentSession _session;
        public IDocumentSession Session
        {
            get
            {
                return this._session;
            }
        }

        public DataContext(string connectionStringName)
        {
            this._documentStore = new DocumentStore
            {
                ConnectionStringName = connectionStringName
            }.Initialize();

            this._session = this._documentStore.OpenSession();
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

        /// <summary>
        /// Attach A Disconnected Data Object To This DataContext
        /// </summary>
        /// <param name="dataObj">Object To Attach</param>
        /// <returns></returns>
        public void Attach(IDataObject dataObj)
        {
            dataObj.Db = this;
        }

        /// <summary>
        /// Attach A Collection of Data Objects To This DataContext
        /// </summary>
        /// <param name="dataObjCollection">Objects To Attach</param>
        /// <returns></returns>
        public void Attach(IEnumerable<IDataObject> dataObjCollection)
        {
            if (dataObjCollection != null)
            {
                foreach (var dataObject in dataObjCollection)
                {
                    dataObject.Db = this;
                }
            }
        }

        public void Dispose()
        {
            this._session.Dispose();
            this._documentStore.Dispose();
        }
    }
}
