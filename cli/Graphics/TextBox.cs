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
            string projectName;

            projectName = Console.ReadLine();
            Console.CursorVisible = false;
            
            return projectName;
        }
    }
}