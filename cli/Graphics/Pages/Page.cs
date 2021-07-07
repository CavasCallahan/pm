using ProjectManager.Commands;

namespace ProjectManager.Graphics.Pages
{
    public class Page
    {
        public Page(GraphInterfaceCommand graph)
        {
            Graph = graph;
        }

        protected GraphInterfaceCommand Graph { get; }

        virtual public void Run()
        {

        }
    }
}