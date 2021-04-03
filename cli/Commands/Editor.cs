using Mono.Options;
using pm.Helpers;
using pm.Models;


namespace ProjectManager.Commands
{
    public enum EditorType{

    }

    public class Editor : Command
    {
        public Editor(ProjectHandler handler) : base("editor", "Change editor")
        {
            Options = new OptionSet{
                { "create", "The Create a new editor", n => Create = n },
                { "p|path", "The path to the editor", n => Path = n },
            };
            Run = arg => { 
                var editor = string.Join(", ", arg);
                ChangeEditor(editor);
             };
            Handler = handler;
        }

        private void ChangeEditor(string editorName)
        {
            //var editors = Handler.ListEditors();

            if (Create?.Length > 0)
            {
                if (Path?.Length > 0)
                {
                    var editor = new EditorModel{
                        Name = Create,
                        Path = Path
                    };
                    Handler.CreateEditor(editor);
                }
                else
                {
                    new MessagesHandler("You have to put the path too", MessageType.Information);
                }
            }
            else
            {
                try
                {
                    Handler.ChangeEditor(editorName);
                }
                catch (System.Exception)
                {
                    new MessagesHandler("Something went wrong!", MessageType.Error);
                }
            }
        }

        public string Create { get; private set; }
        public string Path { get; private set; }
        public ProjectHandler Handler { get; }
    }
}