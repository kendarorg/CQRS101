using Commons;
using Cqrs.SharedContext.Services;
using Cqrs.SharedContext.Services.Dtos;
using System;
using System.Collections.Generic;

namespace TasksManager.Implementation.SharedContext
{
    public class ActivityTypesService :
        IActivityTypesService
    {
        private IRepository<Cqrs.VoContext.Repositories.Entities.ActivityType, string> _repository;

        public ActivityTypesService(IRepository<Cqrs.VoContext.Repositories.Entities.ActivityType, string> repository)
        {
            _repository = repository;
        }

        public IEnumerable<ActivityTypeDto> GetAll()
        {
            foreach (var activityType in _repository.Find())
            {
                ActivityTypeDto result = ActivityTypeToDto(activityType);
                yield return result;
            }
        }

        public ActivityTypeDto GetByCode(string code)
        {
            var result = _repository.GetById(code);
            if (result == null) return null;
            return ActivityTypeToDto(result);
        }

        private static ActivityTypeDto ActivityTypeToDto(Cqrs.VoContext.Repositories.Entities.ActivityType activityType)
        {
            return new ActivityTypeDto
            {
                Code = activityType.Code,
                Description = activityType.Description,
                Name = activityType.Name
            };
        }
    }
}
