using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifying.API.Infrastructure.Services
{
    public interface IIdentityService
    {
        string GetUserIdentity();

        string GetUserName();
    }
}
