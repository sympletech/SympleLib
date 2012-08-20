
namespace SympleLib.RavenDB
{
    public interface IDataObject
    {
        string Id { get; set; }

        DataObjectOperationResult Save();
    }
}