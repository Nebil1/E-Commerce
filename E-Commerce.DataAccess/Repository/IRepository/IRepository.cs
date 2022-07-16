using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T can be any class or category

        T GetFirstorDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
         IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null);   

         void Add(T entity);
         void Remove(T entity);


        //To remove more than one entity
        void RemoveRange(IEnumerable<T> entity); 
    }
}
