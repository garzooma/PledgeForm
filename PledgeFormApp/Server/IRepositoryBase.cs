using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PledgeFormApp.Server
{
  public interface IRepositoryBase<T>
  {
    IEnumerable<T> FindAll();
    T FindByIndex(int index);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
  }
}
