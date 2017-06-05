using Cqrs.Commons;
using Cqrs.LogContext.Commands;
using System;

namespace TasksManager.Implementation.ActivityContext.Validators
{
    public class CreateActivityValidator
        : IValidator<CreateActivity>
    {
        public bool Validate(CreateActivity item)
        {
            if (item.From == null) throw new Exception("Invalid date");
            if (string.IsNullOrWhiteSpace(item.Description)) throw new Exception("Invalid description");
            return true;
        }
    }
}
