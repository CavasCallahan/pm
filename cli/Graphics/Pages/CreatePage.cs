using ProjectManager.Commands;

namespace ProjectManager.Graphics.Pages
{
    public class CreatePage : Page
    {
        public CreatePage(GraphInterfaceCommand graph) : base(graph)
        {

        }

        public override void Run()
        {
            System.Console.WriteLine("Hello Create");
        }
    }
}