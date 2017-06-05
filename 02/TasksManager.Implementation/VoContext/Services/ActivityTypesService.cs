using System.Collections.Generic;
using System.Linq;
using Commons;
using Cqrs.SharedContext.Services;
using Cqrs.SharedContext.Services.Dtos;
using Cqrs.VoContext.Repositories.Entities;

namespace Cqrs.VoContext.Services
{
    public class ActivityTypesService : IActivityTypesService
    {
        private readonly IRepository<ActivityType, string> _repository;

        public ActivityTypesService(IRepository<ActivityType, string> repository)
        {
            _repository = repository;
        }

        public IEnumerable<ActivityTypeDto> GetAll()
        {
            return _repository.
                Find().
                Select(ToDto);
        }

        public ActivityTypeDto GetByCode(string code)
        {
            return ToDto(_repository.GetById(code));
        }

        private static ActivityTypeDto ToDto(ActivityType td)
        {
            if (td == null) return null;
            return new ActivityTypeDto
            {
                Name = td.Name,
                Code = td.Code,
                Description = td.Description
            };
        }
    }
}
