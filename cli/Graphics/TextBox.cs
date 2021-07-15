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
            
            Console.CursorVisible = false;
        }

        public string Run()
        {
            var projectName = Console.ReadLine();

            return projectName;
        }
    }
}