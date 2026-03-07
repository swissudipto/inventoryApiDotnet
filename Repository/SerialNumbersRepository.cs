using inventoryApiDotnet.Model;
using inventoryApiDotnet.Interface;
using Microsoft.EntityFrameworkCore;

namespace inventoryApiDotnet.Repository
{
    public class SerialNumbersRepository : BaseRepository<SerialNumbers>, ISerialNumbersRepository
    {
        protected DbSet<SerialNumbers> _DbSet;

        public SerialNumbersRepository(AppDbContext context) : base(context)
        {
            _DbSet = Context.Set<SerialNumbers>();

        }

        public async Task<SerialNumbers> GetBySerialNumber(string serialnumber)
        {
            return _DbSet.FirstOrDefault(x => x.serial == serialnumber && x.isActive);
        }
    }
}
