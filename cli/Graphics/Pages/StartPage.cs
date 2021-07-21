using pm.Helpers;
using pm.Models;
using ProjectManager.Commands;
using System;
using System.Collections.Generic;

namespace ProjectManager.Graphics.Pages
{
    public class StartPage : Page
    {
        public StartPage(GraphInterfaceCommand graph, ProjectHandler handler) : base(graph)
        {
            Handler = handler;
        }
        public ProjectHandler Handler { get; }

        public override void Run()
        {
            ProjectModel[] projects = Handler.ListProjects();
            List<string> list = new List<string>();

            string[] options = {};

            foreach (var project in projects)
            {
               list.Add(project.Title);
            }

            options = list.ToArray();

            Menu menu = new Menu("Welcome to the new graphical interface!", options);
            menu.addFooter($"\nProject's count: -{list.Count}-      [q]quit     [c]create    \n");
            int selectedIndex = menu.Run();

            // Pressed C key
            if (selectedIndex == options.Length + 1)
            {
                Graph.CreatePage.Run();
            }

            //Pressed Exit Key
            if (selectedIndex == options.Length + 2)
            {
                Environment.Exit(0);
            }

            Graph.ProfilePage.Run(projects[selectedIndex]);
        }
    }
}