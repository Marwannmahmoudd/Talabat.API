using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Core
{
    public interface IUniteOfWork : IDisposable
    {
        IGenericRepository<Tentity> Repository<Tentity>() where Tentity : BaseEntity; 

        Task<int> CompleteAsync(); // As SaveCHanges
    }
}
