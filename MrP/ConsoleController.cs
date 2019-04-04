using System;
using System.Collections.Generic;
using System.Linq;

namespace MrP
{
    public class ConsoleController
    {
        private readonly IList<AConsoleImplementation> consoleApps;

        public ConsoleController()
        {
            consoleApps = AConsoleImplementation.GetSubClasses().ToList();
        }

        public CommandActionEnum Run(string line)
        {
            var splitCmds = line.Split(' ');
            var cmd = splitCmds[0];
            var args = splitCmds.ToList().GetRange(1, splitCmds.Length - 1).ToArray();
            foreach (var ca in consoleApps)
            {
                if (ca.IsCommand(cmd))
                {
                    try
                    {
                        return ca.Run(args);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                        return CommandActionEnum.Continue;
                    }
                }
            }

            Console.Error.WriteLine($"Could not find any listerners for command '{cmd}'");
            return CommandActionEnum.Continue;
        }
    }

    public enum CommandActionEnum
    {
        Crash = -1,
        Exit = 0,
        Continue = 1,
    }

    public abstract class AConsoleImplementation
    {
        public string[] Commands { get; }

        public AConsoleImplementation(params string[] cmds)
        {
            Commands = cmds;
        }

        public bool IsCommand(string cmd)
        {
            return Commands.Any(x => x.Equals(cmd, StringComparison.CurrentCultureIgnoreCase));
        }

        public static IEnumerable<AConsoleImplementation> GetSubClasses()
        {
            return typeof(AConsoleImplementation)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(AConsoleImplementation)) && !t.IsAbstract)
                .Select(t => (AConsoleImplementation)Activator.CreateInstance(t));
        }

        public abstract CommandActionEnum Run(string[] args);

        public void WriteLine(string msg)
        {
            Console.WriteLine($"> {msg}");
        }

        public void WriteLine(object msg)
        {
            WriteLine(msg.ToString());
        }
    }
}
