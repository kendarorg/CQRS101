using Commons;
using System;
using System.Collections.Generic;
using TasksManager.SharedContext.VOs;
using TasksManager.SharedContext.VOs.Entities;
using TasksManager.VOs.Entities;

namespace TasksManager.Implementation.SharedContext
{
    public class ActivityTypesService :
        IActivityTypesService
    {
        private IRepository<ActivityType, string> _repository;

        public ActivityTypesService(IRepository<ActivityType, string> repository)
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

        private static ActivityTypeDto ActivityTypeToDto(ActivityType activityType)
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
