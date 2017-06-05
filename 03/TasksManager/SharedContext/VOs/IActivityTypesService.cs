using System.Collections.Generic;
using TasksManager.SharedContext.VOs.Entities;

namespace TasksManager.SharedContext.VOs
{
    public interface IActivityTypesService
    {
        IEnumerable<ActivityTypeDto> GetAll();
        ActivityTypeDto GetByCode(string code);
    }
}
