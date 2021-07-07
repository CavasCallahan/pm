using System;
using ProjectManager.Commands;

namespace ProjectManager.Graphics.Pages
{
    public class WarningPage : Page
    {
        public WarningPage(GraphInterfaceCommand graph) : base(graph)
        {

        }

        public void Run(Action function)
        {
            string[] options = { "Yes", "No" };

            Menu menu = new Menu("\n Are you sure that you wanna do this? \n", options);
            int selectedIndex = menu.Run();

            switch (selectedIndex)
            {
                case 0:
                    function();
                    break;
                case 1:
                    Graph.StartPage.Run();
                    break;
            }
        }
    }
}