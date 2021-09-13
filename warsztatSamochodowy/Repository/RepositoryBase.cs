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


        virtual public T FormatModel(T unformated)
        {

            IModelFormattable formatable = unformated as IModelFormattable;
            if (formatable != null)
                formatable.FormatMe();
            return unformated;
        }

        virtual public bool CanInsert(T inerted)
        {
            return true;
        }

        virtual public async Task<List<T>> GetListAsync()
        {
            return await dbSet.ToListAsync();
        }
        virtual public List<T> GetList()
        {
            return dbSet.ToList();
        }


        virtual public async Task<int> AddAsync(T added)
        {
            FormatModel(added);
            if (CanInsert(added) == false)
            {
                throw new RepositoryException("Failed To insert Item");
            }



            await dbSet.AddAsync(added);
            return await context.SaveChangesAsync();
        }

        virtual public int Add(T added)
        {
            FormatModel(added);
            if (CanInsert(added) == false)
            {
                throw new RepositoryException("Failed To insert Item");
            }

            dbSet.Add(added);
            return context.SaveChanges();
        }


        virtual public async Task<int> AddRangeAsync(IEnumerable<T> added)
        {

            foreach (T item in added)
            {
                FormatModel(item);
                if (CanInsert(item) == false)
                {
                    throw new RepositoryException("Failed To insert Items");
                }
                await dbSet.AddAsync(item);
            }

            
            return await context.SaveChangesAsync();
        }

        virtual public int AddRange(IEnumerable<T> added)
        {
            foreach(T item in added)
            {
                FormatModel(item);
                if (CanInsert(item) == false)
                {
                    throw new RepositoryException("Failed To insert Items");
                }
                dbSet.Add(item);
            }

            
            return context.SaveChanges();
        }



        virtual public async Task<int> RemoveAsync(T removedEntity)
        {
            dbSet.Remove(removedEntity);
            return await context.SaveChangesAsync();
        }

        virtual public int Remove(T removedEntity)
        {
            dbSet.Remove(removedEntity);
            return context.SaveChanges();
        }



        virtual public async Task<int> RemoveRangeAsync(IEnumerable<T> removedEntities)
        {
            dbSet.RemoveRange(removedEntities);
            return await context.SaveChangesAsync();
        }


        virtual public  int RemoveRange(IEnumerable<T> removedEntities)
        {
            dbSet.RemoveRange(removedEntities);
            return context.SaveChanges();
        }


        virtual public async Task<int> UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            return await context.SaveChangesAsync();
        }

        virtual public int Update(T entity)
        {
            dbSet.Update(entity);
            return context.SaveChanges();
        }


    }
}
