using Mono.Options;
using pm.Helpers;
using System.IO;

namespace ProjectManager.Commands
{
    public class Settings : Command
    {
        public Settings(SettingsHandler handler, ProjectHandler project) : base("settings", "Open's the appsettings.json")
        {
            Run = arg => { 
                SettingsCommand();   
             };
            Handler = handler;
            Project = project;
        }
        public SettingsHandler Handler { get; }
        public ProjectHandler Project { get; }

        private void SettingsCommand()
        {
            var root = Path.GetDirectoryName(Handler.GetSettingsPath());
            Project.OpenCurrentEditor(root, "appsettings.json");
        }
    }


}