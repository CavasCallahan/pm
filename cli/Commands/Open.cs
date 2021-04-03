using Mono.Options;
using pm.Helpers;

namespace ProjectManager.Commands
{
    public class Open : Command
    {
        public Open(ProjectHandler handler) : base("open", "Open's a project")
        {
            Options = new OptionSet{
                { "n|name=", "The name of the project", n => ProjectName = n }, 
                { "t|type=", "The type of the project", t => Type = t },
            };
            Run = arg => { 
                var project = string.Join(", ", arg);

                OpenProject(project);

             };
            Handler = handler;
        }

        public string ProjectName { get; private set; }
        public string Type { get; private set; }
        public ProjectHandler Handler { get; }

        private void OpenProject(string project)
        {
            Handler.OpenProjectHandler(project);
        }
    }
}
