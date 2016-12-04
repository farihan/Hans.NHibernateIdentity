using Hans.Identity.Data.Domains;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Hans.Identity.Data.Persistence
{
    public class RoleStore<TDomain> : IRoleStore<TDomain>, IQueryableRoleStore<TDomain> where TDomain : IdentityRole
    {
        private readonly IRepository<TDomain> repository;

        public RoleStore(ISession session)
        {
            repository = new Repository<TDomain>(session);
        }

        public Task CreateAsync(TDomain role)
        {
            repository.Save(role);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(TDomain role)
        {
            repository.Delete(role);
            return Task.FromResult(0);
        }

        public Task<TDomain> FindByIdAsync(string roleId)
        {
            return Task.FromResult(repository.FindOneBy(x => x.Id.ToLower() == roleId.ToLower()));
        }

        public Task<TDomain> FindByNameAsync(string roleName)
        {
            return Task.FromResult(repository.FindOneBy(x => x.Name.ToLower() == roleName.ToLower()));
        }

        public Task UpdateAsync(TDomain role)
        {
            repository.Update(role);
            return Task.FromResult(0);
        }

        public IQueryable<TDomain> Roles
        {
            get
            {
                return repository.FindAll();
            }
        }

        public void Dispose()
        {
        }
    }
}
