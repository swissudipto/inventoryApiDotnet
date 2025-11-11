using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventoryApiDotnet.Interface
{
    public interface IUnitOfWork
    {
        public Task SaveAsync();
    }
}