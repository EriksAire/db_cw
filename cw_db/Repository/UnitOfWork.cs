﻿using cw_db.Data;
using cw_db.Interfaces;
using System.Collections;

namespace cw_db.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private Hashtable _repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IRepository<T> Repo<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repoType = typeof(Repository<>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(T)), context);
                _repositories.Add(type, repoInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

    }
}
