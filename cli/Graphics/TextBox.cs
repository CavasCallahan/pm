using System;
using pm.Helpers;

namespace ProjectManager
{
    public class TextBox
    {
        public TextBox(string prompt)
        {
            DisplayTextBox();
        }

        private void DisplayTextBox()
        {
            new MessagesHandler("Write the Name of the Project!", MessageType.Information);
            Console.CursorVisible = true;
        }

        public string Run()
        {
            ConsoleModifiers KeyPressed;
            string projectName;

            while ((projectName = Console.ReadLine()) != null)
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler(ListennerHandler);
            }

            return projectName;
        }

        private void ListennerHandler(object sender, ConsoleCancelEventArgs e)
        {
            
        }
    }
}