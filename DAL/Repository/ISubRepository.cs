using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface ISubRepository
    {
        IQueryable<Audio> FindWithExpressionTreeSubAudio(Expression<Func<Sub,Boolean>> predicate);
    }
}
