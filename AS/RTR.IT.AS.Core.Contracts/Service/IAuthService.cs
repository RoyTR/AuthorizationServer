﻿using RTR.IT.AS.Core.Entities.App;
using RTR.IT.AS.Core.Entities.Filter;
using System;
using System.Collections.Generic;

namespace RTR.IT.AS.Core.Contracts.Service
{
    public interface IAccountService : IDisposable
    {
        Usuario GetUserAccount(Filter filter);
        List<Tarea> GetUserTasks(Filter filter);
    }
}
