using System.Collections.Generic;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IRepository<T> where T:class
    {
        IEnumerable<T> List { get; }
        T Add(T entity);
        bool Delete(T entity);
        T Update(T entity);
        T FindById(int Id);
  
    }

}
