using System;
using System.Collections.Generic;
using Tasks.Repositories;
using Utils;

namespace Commons.Services
{
    public interface ITasksService: ISingleton
    {
        TaskDao GetById(Guid id);
        List<TaskDao> GetAll();
    }
}
