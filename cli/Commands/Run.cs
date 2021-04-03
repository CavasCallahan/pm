using Mono.Options;
using pm.Helpers;
using pm;

namespace ProjectManager.Commands
{
    public class Run : Command
    {
        public Run(ProjectHandler handler) : base("run", "Run a command on the file settings.pm.json")
        {
            Run = arg => { 
                var command = string.Join(", ", arg);

                if (command.Length > 0)
                {
                    RunCommand(command);   
                }
                else
                {
                    new MessagesHandler("You have to especify want command you gonna run!", MessageType.Normal);
                }
             };
            Handler = handler;
        }

        private void RunCommand(string command)
        {
            Handler.RunProjectCommand(command);
        }

        public ProjectHandler Handler { get; }
    }


}