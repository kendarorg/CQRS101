using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Repository.Interfaces
{
    public class IRepositoryHelperFactory : ISingleton
    {
        private IRepositoryHelper repositoryHelper;
        private ConcurrentDictionary<Type, IRepositoryHelper> repositoryHelperInstances;

        public IRepositoryHelperFactory(IRepositoryHelper repositoryHelper)
        {
            this.repositoryHelper = repositoryHelper;
            repositoryHelperInstances = new ConcurrentDictionary<Type, IRepositoryHelper>();
        }

        public IRepositoryHelper getHelper(Type clazz)
        {
            if (!repositoryHelperInstances.ContainsKey(clazz))
            {
                repositoryHelperInstances[clazz] = repositoryHelper.Create(clazz);
            }
            return repositoryHelperInstances[clazz];
        }
    }
}
