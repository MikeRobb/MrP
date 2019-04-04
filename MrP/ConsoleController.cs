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
            var splitCmds = line.ToLower().Split(' ');
            foreach (var ca in _consoleApps)
            {
                int foundCmd = ca.IsCommand(splitCmds);
                if (foundCmd >= 0)
                {
                    try
                    {
                        return ca.Run(splitCmds.Range(foundCmd));
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                        return CommandActionEnum.Continue;
                    }
                }
            }

            Console.Error.WriteLine($"Could not find any listerners for command '{line}'");
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

        public int IsCommand(string[] cmds)
        {
            foreach (var x in Commands)
            {
                var curCmds = x.Split(' ');
                var equals = curCmds.Equals<string, string>(cmds.Range(0, curCmds.Length));
                if (equals)
                    return curCmds.Length;
            }

            return -1;
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
