﻿using Fifth.DataAccess.Interfaces;
using Fifth.Models;

namespace Fifth.Interfaces.DataAccess
{
    public interface ISessionDataRepository : IRepository<SessionData, int>
    {
    }
}