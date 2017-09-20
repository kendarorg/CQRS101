using System;
using System.Collections.Generic;
using Tasks.Repositories;
using Utils;

namespace Commons.Services
{
    public interface ITaskTypesService : ISingleton
    {
        TaskTypeDao GetById(String id);
    }
}
