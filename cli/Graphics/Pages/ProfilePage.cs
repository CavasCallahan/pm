using pm.Helpers;
using pm.Models;
using ProjectManager.Commands;

namespace ProjectManager.Graphics.Pages
{
    public class ProfilePage : Page
    {
        public ProfilePage(GraphInterfaceCommand graph, ProjectHandler handler) : base(graph)
        {
            Handler = handler;
        }
        public ProjectHandler Handler { get; }

        public void Run(ProjectModel project)
        {
            string[] options = {"Open","Remove", "Change Editor" , "Exit"};

            Menu menu = new Menu($"\n {project.Title} \n \n Description: \n     {project.Description} \n \n Editor: \n     {project.Editor} \n", options);
            int selectedIndex = menu.Run();

            switch (selectedIndex)
            {
                case 0:
                    Handler.OpenProjectHandler(project.Title);
                    break;
                case 1:
                    Graph.WarningPage.Run(() => Handler.RemoveProject(project.Title));
                    break;
                case 2:
                    System.Console.WriteLine("Working On it!");
                    break;
                case 3:
                    Graph.StartPage.Run();
                    break;
            }
        }
    }
}