using System;
using System.Collections.Generic;
using System.Linq;
using MrP.Core;

namespace MrP
{
    public class ConsoleController
    {
        private readonly IDictionary<string, AConsoleImplementation> _commandToApps;
        private CommandActionEnum _status = CommandActionEnum.Continue;

        public ConsoleController()
        {
            _commandToApps = new Dictionary<string, AConsoleImplementation>();
            foreach (var ca in AConsoleImplementation.GetSubClasses())
            {
                foreach (var c in ca.Commands)
                {
                    if (_commandToApps.ContainsKey(c))
                    {
                        var oldCommand = _commandToApps[c];
                        Console.Error.WriteLine(
                            $"Duplicate commands '{c}' with console apps '{oldCommand.GetType().Name}' and '{ca.GetType().Name}'");
                        _status = CommandActionEnum.Exit;
                        continue;
                    }

                    _commandToApps[c] = ca;
                }
            }
        }

        public void Run()
        {
            Console.WriteLine("Ready!");
            while (_status == CommandActionEnum.Continue)
            {
                _status = ExecuteLine(Console.ReadLine());
            }

            if (_status == CommandActionEnum.Crash)
                Console.Error.WriteLine("Crash!");
        }

        private CommandActionEnum ExecuteLine(string line)
        {
            var splitCmds = line.ToLower().Split(' ');
            foreach (var ca in _commandToApps.OrderByDescending(x => x.Key.Length))
            {
                var curCmd = ca.Key.Split(' ');
                if (curCmd.Equals<string, string>(splitCmds.Range(0, curCmd.Length)))
                {
                    try
                    {
                        return ca.Value.Run(splitCmds.Range(curCmd.Length));
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
