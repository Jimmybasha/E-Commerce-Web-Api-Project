using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Specifications;
using Store.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Repository
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext context;

        public GenericRepository(StoreDbContext context)
        {
            this.context = context;
        }




        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return (IEnumerable<TEntity>) await context.Products.Include(P=>P.Brand).Include(P=>P.Type).ToListAsync();
            }
            return await context.Set<TEntity>().ToListAsync();
        }


        public async Task<TEntity> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return  await context.Products.Include(P=>P.Brand).Include(P=>P.Type).FirstOrDefaultAsync(P=> P.Id==id as int?) as TEntity;
            }
            return await context.Set<TEntity>().FindAsync(id);
        }

        //With Spec

        public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity,TKey> spec)
        {
                return await ApplySpecification(spec).ToListAsync();
        }

        //End of Spec
        public async Task AddAsync(TEntity entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            context.Update(entity);

        }
        public void Delete(TEntity entity)
        {
            context.Remove(entity);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity,TKey> spec)
        {
            return SpecificationsEvaluator<TEntity,TKey>.GetQuery(context.Set<TEntity>(), spec);
        }

        public async Task<int> getCountAsync(ISpecifications<TEntity, TKey> specs)
        {
            return await ApplySpecification(specs).CountAsync();
        }
    }
}
