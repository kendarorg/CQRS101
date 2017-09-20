using Commons.Tasks;
using Cqrs;
using log4net;
using TasksManager.Repositories;

namespace TasksManager
{
    public class ToDoTasksEventHandler : IMessageHandler
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ToDoTasksEventHandler));
        private IRepository<ToDoTaskDao> _repository;
        private IBus _bus;

        public void Register(IBus bus)
        {
            _bus = bus;
            _bus.RegisterHandler(m => Handle((TaskCreated)m), typeof(TaskCreated));
            _bus.RegisterHandler(m => Handle((TaskPriorityChanged)m), typeof(TaskPriorityChanged));
            _bus.RegisterHandler(m => Handle((TaskTitleChanged)m), typeof(TaskTitleChanged));
            _bus.RegisterHandler(m => Handle((TaskCompleted)m), typeof(TaskCompleted));
        }

        public ToDoTasksEventHandler(IRepository<ToDoTaskDao> repository)
        {
            _repository = repository;
        }

        public void Handle(TaskCreated message)
        {
            _logger.Info("TaskCreated");
            var toDoTask = new ToDoTaskDao
            {
                Id = message.Id,
                CreationDate = message.CreationDate
            };
            _repository.Save(toDoTask);
            Handle(message.TitleSet);
            Handle(message.PrioritySet);
        }

        public void Handle(TaskPriorityChanged message)
        {
            _logger.Info("TaskPriorityChanged");
            var toDoTask = _repository.GetById(message.Id);
            toDoTask.Priority = message.New;
            _repository.Save(toDoTask);
        }

        public void Handle(TaskTitleChanged message)
        {
            _logger.Info("TaskTitleChanged");
            var toDoTask = _repository.GetById(message.Id);
            toDoTask.Title = message.New;
            _repository.Save(toDoTask);
        }

        public void Handle(TaskCompleted message)
        {
            _logger.Info("TaskCompleted");
            _repository.Delete(message.Id);
        }
    }
}
