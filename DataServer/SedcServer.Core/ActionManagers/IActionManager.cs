using System;
using System.Linq;

namespace SedcServer.ActionManagers
{
    public interface IActionManager
    {
        string ActionName { get;}

        ProcessResult Process(RequestParser parser);
    }
}
