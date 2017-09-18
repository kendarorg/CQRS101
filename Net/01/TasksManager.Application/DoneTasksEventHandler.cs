using Commons.Services;
using Commons.Tasks;
using Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Repositories;
using TasksManager.Repositories;

namespace TasksManager
{
    public class DoneTasksEventHandler : IMessageHandler
    {
        private ITasksService _tasksService;
        private IRepository<DoneTaskDao> _repository;
        private IBus _bus;

        public void Register(IBus bus)
        {
            _bus = bus;
            _bus.RegisterHandler(m => Handle((TaskCompleted)m), typeof(TaskCompleted));
        }

        public DoneTasksEventHandler(ITasksService tasksService, IRepository<DoneTaskDao> repository)
        {
            _repository = repository;
            _tasksService = tasksService;
        }

        public void Handle(TaskCompleted message)
        {
            var taskDao = _tasksService.GetById(message.Id);
            var doneTask = new DoneTaskDao
            {
                Id = taskDao.Id,
                Priority = taskDao.Priority,
                CreationDate = taskDao.CreationDate,
                Title = taskDao.Title,
                CompletionDate = message.CompletionDate
            };
        }
    }
}
