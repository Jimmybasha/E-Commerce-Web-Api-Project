using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Specifications
{
    public interface ISpecifications<Tentity, Tkey> where Tentity : BaseEntity<Tkey>
    {

        //To Make the Include and where Dynamic for every class

        //Criteria if i want to get the product by id for example
        public Expression<Func<Tentity, bool>> Criteria { get; set; }

        //The .Includes() method 
        public List<Expression<Func<Tentity, object>>> Includes { get; set; }
        public Expression<Func<Tentity, object>> OrderBy { get; set; }
        public Expression<Func<Tentity, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public bool isPaginationEnabled { get; set; }




        //return  await context.Products.Where(P=>P.Id==Id as int?).Include(p => p.Brand).Include(p => p.Type).FirstOrDefaultAsync() as TEntity ;

    }
}
