using inventoryApiDotnet.Model;

namespace inventoryApiDotnet.Interface
{
    public interface ISerialNumbersRepository : IRepository<SerialNumbers>
    {
        Task<SerialNumbers> GetBySerialNumber(string serialnumber);
    }
}