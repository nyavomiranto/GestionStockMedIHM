using GestionStockMedIHM.Interfaces;
using GestionStockMedIHM.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionStockMedIHM.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;

        public Repository(AppDbContext appDbContext) 
        {
           _appDbContext = appDbContext;         
        }

        public async Task<T> GetByIdAsync (int id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _appDbContext.Set<T>().ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
                await _appDbContext.Set<T>().AddAsync(entity);
                await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync (T entity)
        {
            _appDbContext.Set<T>().Update(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _appDbContext.Set<T>().Remove(entity);
                await _appDbContext.SaveChangesAsync();
            }      
        }
    }
}
