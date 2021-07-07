using Mono.Options;
using ProjectManager.Graphics;

namespace ProjectManager.Commands
{
    public class GraphInterfaceCommand : Command
    {
        public GraphInterfaceCommand() : base("graphics", "Command to activate user interface of pmp")
        {
            Run = args => {
                Start();
            };
        }

        private void Start()
        {
            string[] options = { "Get Started", "Exit" };

            Menu menu = new Menu("Welcome to the new graphical interface!", options);
            menu.Run();

            System.Console.WriteLine("Hello");
        }
    }
}