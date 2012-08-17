using System.Collections.Generic;
using Raven.Client;

namespace SympleLib.RavenDB
{
    public interface IDataContext
    {
        IDocumentSession Session { get; }

        void Attach(IDataObject dataObj);

        void Attach(IEnumerable<IDataObject> dataObjCollection);
    }
}
