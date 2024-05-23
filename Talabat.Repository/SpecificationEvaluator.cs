using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repository
{
    internal static class SpecificationEvaluator<IEntity> where IEntity : BaseEntity
    {
        public static IQueryable<IEntity> GetQuery(IQueryable<IEntity> InputQuery , ISpecification<IEntity> spec) 
        {
            var query = InputQuery;
            if (spec.Criteria is not null)// P => P.Id == Id
                query = query.Where(spec.Criteria);

            if (spec.OrderBy is not null)// P => P.Id == Id
                query = query.OrderBy(spec.OrderBy);

            else if (spec.OrderByDesc is not null)// P => P.Id == Id
                query = query.OrderByDescending(spec.OrderByDesc);


            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            //query = _dbcontext.set<T>().where(p => p.Id == id)
            query = spec.Includes.Aggregate(query, (currentquery, includeExpression) => currentquery.Include(includeExpression));

            return query;
        }
    }
}
