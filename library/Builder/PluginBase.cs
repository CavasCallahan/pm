using System;
using System.Collections.Generic;
using pm.Helpers;

namespace pm.Builder
{
    public class PluginBase : IPlugin
    {
        public PluginBase(IServiceProvider service, IEnumerable<string> args)
        {
            Service = service;
            Args = args;
        }

        public string PluginName { get; set; } = "Example-Name";
        public string PluginDescription { get; set; } = "Example-Description";
        public IServiceProvider Service { get; }
        public IEnumerable<string> Args { get; }

        public ProjectHandler ProjectHandler { get; set; }
        public SettingsHandler SettingsHandler { get; set; }

        public void Execute()
        {
            ProjectHandler handler = (ProjectHandler)Service.GetService(typeof(ProjectHandler));
            ProjectHandler = handler;
            SettingsHandler settings = (SettingsHandler)Service.GetService(typeof(SettingsHandler));
            SettingsHandler = settings;
            // MessagesHandler message = (MessagesHandler)Service.GetService(typeof(MessagesHandler));
            // MessagesHandler = message;
        }
    }
}