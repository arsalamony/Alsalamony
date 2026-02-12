using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class 
{
    public T Find(int  id);

    public bool Add(T entity);

    public bool Update(T entity);

    public bool Delete(int id);
}
