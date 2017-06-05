using System.Collections.Generic;
using Commons;
using Cqrs.SharedContext.Services.Dtos;

namespace Cqrs.SharedContext.Services
{
    public interface IActivityTypesService : IService
    {
        IEnumerable<ActivityTypeDto> GetAll();
        ActivityTypeDto GetByCode(string code);
    }
}