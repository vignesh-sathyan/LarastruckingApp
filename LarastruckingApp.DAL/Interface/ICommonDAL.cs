using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.Interface
{
    public interface ICommonDAL<T>
    {
        IEnumerable<T> List { get; }
        T Add(T entity);
        bool Delete(T entity);
        T Update(T entity);
        T FindById(int Id);
    }
}
