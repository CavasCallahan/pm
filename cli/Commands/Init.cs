using Mono.Options;
using pm.Helpers;
using System.IO;

namespace ProjectManager.Commands
{
    public class Init : Command
    {
        public Init(ProjectHandler handler, SettingsHandler settings) : base("init", "Initialize a project")
        {
            Run = arg => { 
                Initialize();
             };
            Handler = handler;
            Settings = settings;
        }

        public ProjectHandler Handler { get; }
        public SettingsHandler Settings { get; }

        private void Initialize()
        {
            var root = Settings.Location;
            var projectName = new DirectoryInfo(root).Name;

            Handler.InitializeProject(root, projectName);
        }
    }


}