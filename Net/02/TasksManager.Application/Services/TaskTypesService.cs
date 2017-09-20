using Commons.Services;
using Cqrs;
using System;

namespace TasksManager.Services
{
    public class TaskTypesService : ITaskTypesService
    {
        private IRepository<TaskTypeDao, string> _repository;

        public TaskTypesService(IRepository<TaskTypeDao,String> repository)
        {
            _repository = repository;
        }

        public TaskTypeDao GetById(string id)
        {
            return _repository.GetById(id);
        }
    }
}
