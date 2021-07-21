using System;
using System.Collections.Generic;
using pm.Models;

namespace ProjectManager.Graphics
{
    public class Menu : IMenu
    {
        private int SelectedIndex { get; set; }
        private string[] Options { get; set; }
        private string Prompt { get; set; }

        private List<string> DisplayOptionsList {get; set;} = new List<string>();
        
        public Menu(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
        }
        private void DisplayOptions()
        {
            Console.WriteLine(Prompt);

            for (int i = 0; i < (Options.Length); i++)
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

                Console.WriteLine($"<< { currentOption } >>");
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
                DrawOptions();

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
                else if (keyPressed == ConsoleKey.C)
                {
                    return Options.Length + 1;
                }
                else if (keyPressed == ConsoleKey.Q || keyPressed == ConsoleKey.Spacebar)
                {
                    return Options.Length + 2;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex ++;

                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = 0;
                    }
                }

            } while(keyPressed != ConsoleKey.Enter || keyPressed == ConsoleKey.C);

            return SelectedIndex;
        }

        private void DrawOptions()
        {
            foreach (var option in DisplayOptionsList)
            {
                System.Console.WriteLine(option);
            }
        }

        public IMenu addFooter(string prompt)
        {
            DisplayOptionsList.Add(prompt);

            return this;
        }
    }
}