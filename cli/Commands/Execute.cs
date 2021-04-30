using System.Collections.Generic;
using System.Reflection;
using Mono.Options;
using ProjectManager.Helpers;

namespace ProjectManager.Commands
{
    public class Execute : Command
    {
        public Execute(ExtentionHelper helper) : base("exe", "Executes an extention")
        {
             bool shouldShowHelp = false;

            var options = new OptionSet{ 
                { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
            };

            Options = options;
            Run = arg => { 
                if (shouldShowHelp)
                {
                    System.Console.WriteLine ("Options:");
                    System.Console.WriteLine ("Arguments:");
                    System.Console.WriteLine();
                    System.Console.WriteLine ("Pm exec [ExtentionName]");
                    System.Console.WriteLine();
                    options.WriteOptionDescriptions(System.Console.Out);
                }else{
                    var extention = string.Join(", ", arg);
                    RunExecute(extention, arg);
                }
             };
            Helper = helper;
        }

        public ExtentionHelper Helper { get; }

        private void RunExecute(string name, IEnumerable<string> args)
        {
            var extentions = Helper.ReadExtention();
            
            foreach (KeyValuePair<string, Assembly> extention in extentions)
            {
                if (extention.Key == name)
                {
                    Helper.ExecuteExtention(extention.Key, extention.Value, args);
                }
            }
        }
    }
}