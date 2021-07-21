using Mono.Options;
using ProjectManager.Commands;
using pm.Helpers;
using pm.Models;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace ProjectManager
{
    public class StartService
    {
        public StartService(Create create, Init init, Open open, Editor editor, Remove remove, Run runCommand, SettingsHandler handler,
        Settings settings, Execute execute, GraphInterfaceCommand graph)
        {
            Create = create;
            Init = init;
            Open = open;
            Editor = editor;
            Remove = remove;
            RunCommand = runCommand;
            Handler = handler;
            Settings = settings;
            Execute = execute;
            Graph = graph;
        }

        public Create Create { get; }
        public Init Init { get; }
        public Open Open { get; }
        public Editor Editor { get; }
        public Remove Remove { get; }
        public Run RunCommand { get; }
        public SettingsHandler Handler { get; }
        public Settings Settings { get; }
        public Execute Execute { get; }
        public GraphInterfaceCommand Graph { get; }

        public void Run(string[] args, string root)
        {
            Handler.Location = root;

            //Fix's the directory hidden
            if (!File.Exists(Handler.PathToPmDirectory) && !File.Exists(Handler.PathToPmDirectory + "/redirect.json"))
            {
                var pmDir = new DirectoryInfo(Handler.PathToPmDirectory);
                pmDir.Create();
                pmDir.Attributes = FileAttributes.Hidden;

                var list = new List<string>();

                var example = new RedirectModel{
                    Redirect = list
                };
                
                var options = new JsonSerializerOptions{
                    WriteIndented = true
                };

                var jsonString = JsonSerializer.Serialize<RedirectModel>(example, options);
                File.WriteAllText($"{ Handler.PathToPmDirectory }\\redirect.json",jsonString);
            }

            //Commands to run
            var commands = new CommandSet("commands"){
                Create,
                Init,
                Open,
                Editor,
                Remove,
                RunCommand,
                Settings,
                Execute,
                Graph
            };

            try {

                commands.Run(args);

            } catch (System.Exception) {
                return;
            }
        }
    }
}