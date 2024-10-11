 using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Store.Core.Specifications
{
    public class SpecificationsEvaluator<TEntity,TKey> where TEntity:BaseEntity<TKey>
    {

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity,TKey> spec)
        {
            //context.Products() for example
            var query = inputQuery;

            //For where Func
            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }


            if(spec.OrderBy is not null)
            {
             query  =  query.OrderBy(spec.OrderBy);
            }
            

            if(spec.OrderByDescending is not null)
            {
                query = query.OrderBy(spec.OrderByDescending);
            }

            if (spec.isPaginationEnabled)
            {
                Console.WriteLine($"Applying pagination skip : Skip[{spec.Skip}] and take{spec.Take}");
                query = query.Skip(spec.Skip).Take(spec.Take);

            }

            //To make the Includes Take all the Includes inside it bcz it's a list so i'll iterate with the Aggregate for
            //Every Include Func

            //P=>P.brand
            //P=>P.types

            //_context.Products.Include(P=>p.Producttype) 1st Iteration
            //_context.Products.Include(P=>p.Producttype).Include(P=>P.Bramd);
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression) );

            return query;

        }


    }
}
