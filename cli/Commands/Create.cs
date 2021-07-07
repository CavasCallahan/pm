using Mono.Options;
using System;
using pm.Helpers;
using System.IO;

namespace ProjectManager.Commands
{
    public class Create : Command
    {
        public Create(ProjectHandler handler, SettingsHandler settings) : base("new", "Create's new project with multiple Templates avaible by pm")
        {
            bool shouldShowHelp = false;

            var options = new OptionSet{ 
                { "t|type=", "The type of the project", t => Type = t },
                { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
            };

            Options = options;
            Run = arg => { 
                var project = string.Join(", ", arg);

                if (shouldShowHelp)
                {
                    Console.WriteLine ("Options:");
                    options.WriteOptionDescriptions(Console.Out);
                }else{
                    if (project.Length > 1)
                    {
                        CreateProject(project, Type);
                    }
                    else
                    {
                        System.Console.WriteLine("pmp create ['Project-Name'] -t ['Template']");
                        System.Console.WriteLine();
                        System.Console.WriteLine("Templates Avaible: ");
                        System.Console.WriteLine();

                        string[] templates = Directory.GetDirectories(Settings.GetTemplatePath());
                        
                        foreach (var template in templates)
                        {
                            System.Console.WriteLine($"=> { Path.GetFileName(template).ToLower() }");
                            System.Console.WriteLine();
                        }
                    }
                }
             };
            Handler = handler;
            Settings = settings;
        }
        public string Type { get; private set; }
        public ProjectHandler Handler { get; }
        public SettingsHandler Settings { get; }

        private void CreateProject(string projectName, string Type = null)
        {
            if(Type != null)
            {
                switch (Type)
                {
                    case "csharp":
                        Handler.CreateProject(projectName, ProjectType.Csharp);
                    break;
                    case "python":
                        Handler.CreateProject(projectName, ProjectType.Python);
                    break;
                    case "plugin":
                        Handler.CreateProject(projectName, ProjectType.Plugin);
                    break;
                    case "command":
                        Handler.CreateProject(null, ProjectType.Command, projectName);
                    break;
                }
            }
            else
            {
                Handler.CreateProject(projectName, ProjectType.Simple);
            }
        }
    }


}