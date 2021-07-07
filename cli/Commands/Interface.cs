using Mono.Options;
using System;
using ProjectManager.Graphics.Pages;
using pm.Helpers;

namespace ProjectManager.Commands
{
    public class GraphInterfaceCommand : Command
    {
        public GraphInterfaceCommand(ProjectHandler Handler) : base("gh", "Command to activate user interface of pmp")
        {
            Run = args => {
                Start();
            };
            this.Handler = Handler;

            StartPage = new StartPage(this, Handler);
            ProfilePage = new ProfilePage(this, Handler);
            WarningPage = new WarningPage(this);
        }

        public ProjectHandler Handler { get; }
        public StartPage StartPage { get; }
        public ProfilePage ProfilePage { get; }
        public WarningPage WarningPage { get; }

        private void Start()
        {
            Console.CursorVisible = false;
            StartPage.Run();
            Console.CursorVisible = true;
        }
    }
}