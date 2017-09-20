using Commons.Services;
using Commons.Tasks;
using Cqrs;
using log4net;
using System;
using Tasks.Repositories;
using TasksManager.Repositories;

namespace TasksManager
{
    public class TaskTypesStatsEventHandler : IMessageHandler
    {
        private static ILog _logger = LogManager.GetLogger(typeof(TaskTypesStatsEventHandler));
        private ITasksService _tasksService;
        private IRepository<TaskTypeStatDao, String> _repository;
        private IBus _bus;

        public void Register(IBus bus)
        {
            _bus = bus;
            _bus.RegisterHandler(m => Handle((TaskCompleted)m), typeof(TaskCompleted));
            _bus.RegisterHandler(m => Handle((TaskCreated)m), typeof(TaskCreated));
        }

        public TaskTypesStatsEventHandler(ITasksService tasksService, IRepository<TaskTypeStatDao, String> repository)
        {
            _repository = repository;
            _tasksService = tasksService;
        }

        public void Handle(TaskCompleted message)
        {
            _logger.Info("TaskCompleted");
            TaskDao taskDao = _tasksService.GetById(message.Id);
            String code = taskDao.TypeCode == null ? "NONE" : taskDao.TypeCode;

            TaskTypeStatDao currentStat = _repository.GetById(code);
            if (currentStat == null)
            {
                throw new IndexOutOfRangeException("Missing stats for code " + code);
            }

            currentStat.Completed = currentStat.Completed + 1;
            currentStat.Running = currentStat.Running - 1;

            _repository.Save(currentStat);
        }

        public void Handle(TaskCreated message)
        {
            _logger.Info("TaskCreated");
            TaskDao taskDao = _tasksService.GetById(message.Id);
            String code = taskDao.TypeCode == null ? "NONE" : taskDao.TypeCode;

            TaskTypeStatDao currentStat = _repository.GetById(code);
            if (currentStat == null)
            {
                currentStat = new TaskTypeStatDao();
                currentStat.TypeCode = code;
            }

            currentStat.Running = currentStat.Running + 1;

            _repository.Save(currentStat);
        }
    }
}
