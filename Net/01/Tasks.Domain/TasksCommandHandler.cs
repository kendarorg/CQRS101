using System;
using Cqrs;
using Tasks.Commands;
using Tasks.Repositories;
using Commons.Tasks;

namespace Tasks
{
    public class TasksCommandHandler : IMessageHandler
    {
        private IRepository<TaskDao> _repository;
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

        public TasksCommandHandler(IRepository<TaskDao> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateTask message)
        {
            var now = DateTime.Now;
            var taskDao = new TaskDao()
            {
                Id = message.Id,
                Description = message.Description,
                DueDate = message.DueDate,
                Priority = message.Priority,
                Title = message.Title,
                Completed = false,
                CreationDate = now
            };

            _repository.Save(taskDao);

            _bus.Send(new TaskCreated
            {
                Id = taskDao.Id,
                CreationDate = now,
                TitleSet = new TaskTitleChanged
                {
                    Id = taskDao.Id,
                    New = taskDao.Title
                },
                DescriptionSet = new TaskDescriptionChanged
                {
                    Id = taskDao.Id,
                    New = taskDao.Description
                },
                PrioritySet = new TaskPriorityChanged
                {
                    Id = taskDao.Id,
                    New = taskDao.Priority
                },
                DueDateSet = new TaskDueDateChanged
                {
                    Id = taskDao.Id,
                    New = taskDao.DueDate
                }
            });
        }

        public void Handle(ChangeTaskPriority command)
        {
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
    }
}
