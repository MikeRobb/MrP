using System;
using System.Collections.Generic;
using System.Linq;
using MrP.Core;

namespace MrP
{
    public class ConsoleController
    {
        private readonly IList<AConsoleImplementation> _consoleApps;

        public ConsoleController()
        {
            _consoleApps = AConsoleImplementation.GetSubClasses().ToList();
        }

        public void Run()
        {
            Console.WriteLine("Checking commands are unique");
            CommandActionEnum curAction = CommandsAreValid();

            bool firstRun = true;
            while (curAction == CommandActionEnum.Continue)
            {
                if (firstRun)
                {
                    firstRun = false;
                    Console.WriteLine("Ready!");
                }

                curAction = ExecuteLine(Console.ReadLine());
            }

            if (curAction == CommandActionEnum.Crash)
                Console.Error.WriteLine("Crash!");
        }

        private CommandActionEnum CommandsAreValid()
        {
            var registeredCommands = new HashSet<string>();
            foreach (var ap in _consoleApps)
            {
                foreach (var c in ap.Commands)
                {
                    var isUnique = registeredCommands.Add(c);
                    if (isUnique == false)
                    {
                        Console.Error.WriteLine($"Command {c} has been defined in multiple applications");
                        return CommandActionEnum.Exit;
                    }
                }
            }

            return CommandActionEnum.Continue;
        }

        private CommandActionEnum ExecuteLine(string line)
        {
            var splitCmds = line.Split(' ');
            var cmd = splitCmds[0];
            var args = splitCmds.Range(1);
            foreach (var ca in _consoleApps)
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
