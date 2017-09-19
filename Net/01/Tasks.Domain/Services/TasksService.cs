using Commons.Services;
using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Repositories;

namespace Tasks.Services
{
    public class TasksService : ITasksService
    {
        private IRepository<TaskDao> _repository;

        public TasksService(IRepository<TaskDao> repository)
        {
            _repository = repository;
        }
        public List<TaskDao> GetAll()
        {
            return _repository.GetAll();
        }

        public TaskDao GetById(Guid id)
        {
            return _repository.GetById(id);
        }
    }
}
