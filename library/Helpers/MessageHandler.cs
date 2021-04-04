using System;

namespace pm.Helpers
{
    public enum MessageType{
        Error,
        Normal,
        Information
    }
    public class MessagesHandler
    {
        public MessagesHandler(string message, MessageType type)
        {
            var oldColor = Console.ForegroundColor;

            switch (type)
            {
                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case MessageType.Normal:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case MessageType.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            System.Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}