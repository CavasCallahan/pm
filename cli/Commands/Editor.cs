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

            if (editorName.Length > 0)
            {
                if (Path?.Length > 0)
                {
                    var editor = new EditorModel
                    {
                        Name = editorName,
                        Path = Path       
                    };

                    Handler.CreateEditor(editor);
                }
                else
                {
                    var editor = editorName.Split(", ");
                    Handler.ChangeEditor(editor[0]);    
                }
            }
        }

        public string Create { get; private set; }
        public string Path { get; private set; }
        public ProjectHandler Handler { get; }
    }
}