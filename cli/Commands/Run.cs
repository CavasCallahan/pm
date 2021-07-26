using Mono.Options;
using pm.Helpers;

namespace ProjectManager.Commands
{
    public class Run : Command
    {
        public Run(ProjectHandler handler) : base("run", "Run a command on the file settings.pm.json")
        {
            bool shouldUseComplex = false;

            Options = new OptionSet{
                { "e|external" , "This switch allow you to use external commands", e => shouldUseComplex = e != null}
            };

            Run = arg => { 
                var command = string.Join(" ", arg);

                if (command.Length > 0)
                {
                    if (shouldUseComplex)
                    {
                        CheckXmlFile(command);
                    }else
                    {
                        RunCommand(command);   
                    }
                }
                else
                {
                    MessagesHandler.Message("You have to especify want command you gonna run!", MessageType.Normal);
                }
             };
            Handler = handler;
        }

        private void CheckXmlFile(string command)
        {
            Handler.OpenXmlFileInProject(command);
        }

        private void RunCommand(string command)
        {
            Handler.RunProjectCommand(command);
        }

        public ProjectHandler Handler { get; }
    }


}