using Mono.Options;
using pm.Helpers;
using System;

namespace ProjectManager.Commands
{
    public class Create : Command
    {
        public Create(ProjectHandler handler) : base("create", "Create's things that the name ok read please")
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
                        new MessagesHandler("You have to write the name of the project", MessageType.Normal);
                    }
                }
             };
            Handler = handler;
        }
        public string Type { get; private set; }
        public ProjectHandler Handler { get; }

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
                        Handler.CreateProject(projectName, ProjectType.python);
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