using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using pm.Helpers;

namespace ProjectManager.Helpers
{
    public class ExtentionHelper
    {
        public SettingsHandler Settings { get; }
        public IServiceProvider Service { get; }

        public ExtentionHelper(SettingsHandler settings, IServiceProvider service)
        {
            Settings = settings;
            Service = service;
        }

        public Dictionary<string, Assembly> DllList { get; private set; } = new Dictionary<string, Assembly>();

        public dynamic ExtentionAssembly { get; private set; }

        public IDictionary<string, Assembly> ReadExtention()
        {
            var path = Settings.GetExtentionPath();

            string[] dlls = Directory.GetFiles(path);

            foreach (var dll in dlls)
            {                
                var extention = Assembly.LoadFile(dll);
                var extentionName = Path.GetFileNameWithoutExtension(dll);

                DllList.Add(extentionName, extention);

            }

            return DllList;
        }

        public void ExecuteExtention(string name, Assembly extention, IEnumerable<string> args)
        {
            foreach (Type type in extention.GetExportedTypes())
            {
                dynamic c = Activator.CreateInstance(type, Service, args);
                c.Execute();
            }
        }
    }
}