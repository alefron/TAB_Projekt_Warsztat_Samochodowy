using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using warsztatSamochodowy.Models;

namespace warsztatSamochodowy.Repository
{
    public abstract class RepositoryBase<T> where T:class
    {
        private MyDbContext _context;
        protected MyDbContext context { get => _context; }
        protected DbSet<T> dbSet;

        public RepositoryBase()
        {
            var contextOptions = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlServer(@"Server=(local)\sqlexpress;Database=TAB_proj_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            _context = new MyDbContext(contextOptions);
        }


        public async Task<List<T>> GetListAsync()
        {
            return await dbSet.ToListAsync();
        }
        public List<T> GetList()
        {
            return dbSet.ToList();
        }


        public async Task<int> AddAsync(T added)
        {
            await dbSet.AddAsync(added);
            return await context.SaveChangesAsync();
        }

        public int Add(T added)
        {
            dbSet.Add(added);
            return context.SaveChanges();
        }


        public async Task<int> AddRangeAsync(IEnumerable<T> added)
        {
            await dbSet.AddRangeAsync(added);
            return await context.SaveChangesAsync();
        }

        public int AddRange(IEnumerable<T> added)
        {
            dbSet.AddRange(added);
            return context.SaveChanges();
        }

    }
}
