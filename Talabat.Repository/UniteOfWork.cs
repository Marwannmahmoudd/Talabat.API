using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UniteOfWork : IUniteOfWork
    {
        private readonly StoreContext _dbcontext;

        private Hashtable _repositories;
        public UniteOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
            _repositories = new Hashtable();
        }

        public IGenericRepository<Tentity> Repository<Tentity>() where Tentity : BaseEntity
        {
            var key = typeof(Tentity).Name;
            if(!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<Tentity>(_dbcontext);
                _repositories.Add(key, repository);
            }
            return _repositories[key] as IGenericRepository<Tentity>;
        }
        public async Task<int> CompleteAsync()
        => await _dbcontext.SaveChangesAsync();

        public async void Dispose()
       => await _dbcontext.DisposeAsync();

       
    }
}
