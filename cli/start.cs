using Mono.Options;
using ProjectManager.Commands;
using pm.Helpers;

namespace ProjectManager
{
    public class StartService
    {
        public StartService(Create create, Init init, Open open, Editor editor, Remove remove, Run runCommand, SettingsHandler handler,
        Settings settings)
        {
            Create = create;
            Init = init;
            Open = open;
            Editor = editor;
            Remove = remove;
            RunCommand = runCommand;
            Handler = handler;
            Settings = settings;
        }

        public Create Create { get; }
        public Init Init { get; }
        public Open Open { get; }
        public Editor Editor { get; }
        public Remove Remove { get; }
        public Run RunCommand { get; }
        public SettingsHandler Handler { get; }
        public Settings Settings { get; }

        public void Run(string[] args, string root)
        {
            Handler.Location = root;

            //Commands to run
            var commands = new CommandSet("commands"){
                Create,
                Init,
                Open,
                Editor,
                Remove,
                RunCommand,
                Settings
            };

            try {
                //Runs the commands
                commands.Run(args);
            } catch (System.Exception) {
                return;
            }
        }
    }
}