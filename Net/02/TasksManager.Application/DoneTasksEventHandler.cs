using Commons.Services;
using Commons.Tasks;
using Cqrs;
using log4net;
using System;
using TasksManager.Repositories;

namespace TasksManager
{
    public class DoneTasksEventHandler : IMessageHandler
    {
        private static ILog _logger = LogManager.GetLogger(typeof(DoneTasksEventHandler));
        private ITasksService _tasksService;
        private IRepository<DoneTaskDao, Guid> _repository;
        private IBus _bus;

        public void Register(IBus bus)
        {
            _bus = bus;
            _bus.RegisterHandler(m => Handle((TaskCompleted)m), typeof(TaskCompleted));
        }

        public DoneTasksEventHandler(ITasksService tasksService, IRepository<DoneTaskDao, Guid> repository)
        {
            _repository = repository;
            _tasksService = tasksService;
        }

        public void Handle(TaskCompleted message)
        {
            _logger.Info("TaskCompleted");
            var taskDao = _tasksService.GetById(message.Id);
            var doneTask = new DoneTaskDao
            {
                Id = taskDao.Id,
                Priority = taskDao.Priority,
                CreationDate = taskDao.CreationDate,
                Title = taskDao.Title,
                CompletionDate = message.CompletionDate
            };
            _repository.Save(doneTask);
        }
    }
}
