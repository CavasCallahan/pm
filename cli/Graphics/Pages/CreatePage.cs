using pm.Helpers;
using System;
using ProjectManager.Commands;

namespace ProjectManager.Graphics.Pages
{
    public class CreatePage : Page
    {
        public CreatePage(GraphInterfaceCommand graph, ProjectHandler handler) : base(graph)
        {
            Handler = handler;
        }
        public ProjectHandler Handler { get; }

        public override void Run()
        {
            Console.Clear();
            
            //TextBox
            new MessagesHandler("Write the Name of the Project!", MessageType.Information);
            Console.CursorVisible = true;
            var projectName = Console.ReadLine();
            Console.CursorVisible = false;

            if (projectName.Length > 0)
            {
                string[] options = { "Normal", "Csharp", "Java", "Plugin", "Python", "Command", "Exit" };

                Menu menu = new Menu("Chose what project type do you wanna use", options);
                
                var index = menu.Run();

                switch (index)
                {
                    case 0:
                        Handler.CreateProject(projectName, ProjectType.Simple);
                        break;
                    case 1:
                        Handler.CreateProject(projectName, ProjectType.Csharp);
                        break;
                    case 2:
                        Handler.CreateProject(projectName, ProjectType.Java);
                        break;
                    case 3:
                        Handler.CreateProject(projectName, ProjectType.Plugin);
                        break;
                    case 4:
                        Handler.CreateProject(projectName, ProjectType.Python);
                        break;
                    case 5:
                        Handler.CreateProject(projectName, ProjectType.Command);
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }
            Environment.Exit(0);
        }
    }
}