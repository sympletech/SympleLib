
using Raven.Imports.Newtonsoft.Json;
namespace SympleLib.RavenDB
{
    public interface IDataObject
    {
        [JsonIgnore]
        IDataContext Db { get; set; }

        string Id { get; set; }

        DataObjectOperationResult Save();
    }
}