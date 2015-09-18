using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SedcServer.ActionManagers
{
    internal class ActionManagerFactory
    {
        private static Dictionary<string, IActionManager> actionManagers;

        static ActionManagerFactory()
        {
            actionManagers = new Dictionary<string, IActionManager>();

            var location = Assembly.GetExecutingAssembly().Location;
            Console.WriteLine(location);
            var path = Path.GetDirectoryName(location);

            var assemblyNames = Directory.GetFiles(path, "*.dll");

            foreach (var assemblyName in assemblyNames)
            {
                if (assemblyName == location)
                    continue;
                try
                {
                    Assembly assembly = Assembly.LoadFile(assemblyName);
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (typeof (IActionManager).IsAssignableFrom(type))
                        {
                            var actionManager = (IActionManager) Activator.CreateInstance(type);
                            actionManagers.Add(actionManager.ActionName.ToLower(), actionManager);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception loading plugins "+ex.Message);
                }
            }

        }

        internal static IActionManager GetManager(RequestParser parser)
        {
            var action = parser.Action.ToLower();
            if (actionManagers.ContainsKey(action))
                return actionManagers[action];

            return new DefaultActionManager();
        }
    }
}