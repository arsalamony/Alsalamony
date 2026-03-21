using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class 
{
    public Task<T> Find(int  id);

    public Task<bool> Add(T entity);

    public Task<bool> Update(T entity);

    public Task<bool> Delete(int id);
}
