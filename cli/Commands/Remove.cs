using Mono.Options;
using pm.Helpers;

namespace ProjectManager.Commands
{
    public class Remove : Command
    {
        public Remove(ProjectHandler handler) : base("rm", "Remove a project!")
        {
            Run = arg => { 
                var project = string.Join(", ", arg);

                RemoveProject(project);
             };
            Handler = handler;
        }

        private void RemoveProject(string project)
        {
            Handler.RemoveProject(project);
        }

        public ProjectHandler Handler { get; }
    }


}