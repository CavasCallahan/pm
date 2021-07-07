using System;
using pm.Models;

namespace ProjectManager.Graphics
{
    public class Menu
    {
        public int SelectedIndex { get; set; }
        public string[] Options { get; set; }
        public string Prompt { get; set; }
        
        public Menu(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
        }

        private void DisplayOptions()
        {
            Console.WriteLine(Prompt);

            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                
                if (i == SelectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"<<{ currentOption }>>");
            }
            Console.ResetColor();
        }

        public int Run()
        {
            ConsoleKey keyPressed;

            do
            {
                Console.Clear();
                DisplayOptions();

                ConsoleKeyInfo KeyInfo = Console.ReadKey(true);

                keyPressed = KeyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex --;

                    if (SelectedIndex == -1)    
                    {
                        SelectedIndex = Options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex ++;

                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = 0;
                    }
                }
                

            } while(keyPressed != ConsoleKey.Enter);

            return SelectedIndex;
        }
    }
}