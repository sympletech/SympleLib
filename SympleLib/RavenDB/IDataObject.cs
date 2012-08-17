
namespace SympleLib.RavenDB
{
    public interface IDataObject
    {
        IDataContext Db { get; set; }
        string Id { get; set; }

        DataObjectOperationResult Save();
    }
}