using System;
using Cqrs;
using Tasks.Commands;
using Tasks.Repositories;
using Commons.Tasks;
using log4net;
using Commons.Services;

namespace Tasks
{
    public class TasksCommandHandler : IMessageHandler
    {
        private static ILog _logger = LogManager.GetLogger(typeof(TasksCommandHandler));
        private ITaskTypesService _taskTypes;
        private IRepository<TaskDao, Guid> _repository;
        private IBus _bus;

        public void Register(IBus bus)
        {
            _bus = bus;
            _bus.RegisterHandler(c => Handle((CreateTask)c), typeof(CreateTask));
            _bus.RegisterHandler(c => Handle((CompleteTask)c), typeof(CompleteTask));

            _bus.RegisterHandler(c => Handle((ChangeTaskTitle)c), typeof(ChangeTaskTitle));
            _bus.RegisterHandler(c=>Handle((ChangeTaskDescription)c),typeof(ChangeTaskDescription));
            _bus.RegisterHandler(c => Handle((ChangeTaskDueDate)c), typeof(ChangeTaskDueDate));
            _bus.RegisterHandler(c => Handle((ChangeTaskPriority)c), typeof(ChangeTaskPriority));
        }

        public TasksCommandHandler(IRepository<TaskDao, Guid> repository,ITaskTypesService taskTypes)
        {
            _taskTypes = taskTypes;
            _repository = repository;
        }

        public void Handle(CreateTask command)
        {
            TaskTypeDao taskType = null;
            if (!string.IsNullOrWhiteSpace(command.TypeCode))
            {
                taskType = _taskTypes.GetById(command.TypeCode);
                if (taskType == null)
                {
                    throw new ArgumentOutOfRangeException("Missing task type " + command.TypeCode);
                }
            }

            _logger.Info("CreateTask");
            var now = DateTime.Now;
            var taskDao = new TaskDao()
            {
                Id = command.Id,
                Description = command.Description,
                DueDate = command.DueDate,
                Priority = command.Priority,
                Title = command.Title,
                Completed = false,
                CreationDate = now,
                TypeCode = taskType==null?null:taskType.Code
            };

            _repository.Save(taskDao);

            var message = new TaskCreated
            {
                Id = taskDao.Id,
                CreationDate = now,
                TitleSet = new TaskTitleChanged(taskDao.Id, taskDao.Title),
                DescriptionSet = new TaskDescriptionChanged(taskDao.Id, taskDao.Description),
                PrioritySet = new TaskPriorityChanged(taskDao.Id, taskDao.Priority),
                DueDateSet = new TaskDueDateChanged(taskDao.Id, taskDao.DueDate),
            };
            if (taskType != null)
            {
                message.TaskTypeSet = new TaskTypeChanged(taskDao.Id, taskDao.TypeCode);
            }
            _bus.Send(message);
        }

        public void Handle(ChangeTaskPriority command)
        {
            _logger.Info("ChangeTaskPriority");
            var taskDao = _repository.GetById(command.Id);
            var message = new TaskPriorityChanged
            {
                Id = command.Id,
                New = command.Priority,
                Old = taskDao.Priority
            };
            taskDao.Priority = command.Priority;
            _repository.Save(taskDao);
            _bus.Send(message);
        }

        public void Handle(ChangeTaskDueDate command)
        {
            _logger.Info("ChangeTaskDueDate");
            var taskDao = _repository.GetById(command.Id);
            var message = new TaskDueDateChanged
            {
                Id = command.Id,
                New = command.DueDate,
                Old = taskDao.DueDate
            };
            taskDao.DueDate = command.DueDate;
            _repository.Save(taskDao);
            _bus.Send(message);
        }

        public void Handle(ChangeTaskDescription command)
        {
            _logger.Info("ChangeTaskDescription");
            var taskDao = _repository.GetById(command.Id);
            var message = new TaskDescriptionChanged
            {
                Id = command.Id,
                New = command.Description,
                Old = taskDao.Description
            };
            taskDao.Description = command.Description;
            _repository.Save(taskDao);
            _bus.Send(message);
        }

        public void Handle(ChangeTaskTitle command)
        {
            _logger.Info("ChangeTaskTitle");
            var taskDao = _repository.GetById(command.Id);
            var message = new TaskTitleChanged
            {
                Id = command.Id,
                New = command.Title,
                Old = taskDao.Title
            };
            taskDao.Title = command.Title;
            _repository.Save(taskDao);
            _bus.Send(message);
        }

        public void Handle(CompleteTask message)
        {
            _logger.Info("CompleteTask");
            var now = DateTime.Now;
            var taskDao = _repository.GetById(message.Id);
            taskDao.Completed = true;
            taskDao.CompletionDate = now;

            _bus.Send(new TaskCompleted
            {
                Id = taskDao.Id,
                CompletionDate = now
            });
        }

        public void Handle(ChangeTaskType command)
        {
            TaskTypeDao taskType = null;
            if (!string.IsNullOrWhiteSpace(command.TypeCode))
            {
                taskType = _taskTypes.GetById(command.TypeCode);
                if (taskType == null)
                {
                    throw new ArgumentOutOfRangeException("Missing task type " + command.TypeCode);
                }
            }

            _logger.Info("ChangeTaskType");
            var taskDao = _repository.GetById(command.Id);
            var message = new TaskTypeChanged
            {
                Id = command.Id,
                New = command.TypeCode,
                Old = taskDao.TypeCode
            };
            taskDao.TypeCode = command.TypeCode;
            _repository.Save(taskDao);
            _bus.Send(message);
        }
    }
}
