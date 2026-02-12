using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User Find(string UserName);

        IEnumerable<User> GetAll();
    }
}
