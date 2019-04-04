using System;
using System.Collections.Generic;

namespace MrP.ConsoleApps
{
    public class Add : BinaryOp
    {
        public Add() : base((x,y) => x+y,"add", "+")
        {

        }
    }

    public class Subtract : BinaryOp
    {
        public Subtract() : base((x,y) => x-y, "sub", "subtract", "-") { }
    }

    public abstract class BinaryOp : AConsoleImplementation
    {
        private readonly Func<decimal, decimal, decimal> _func;

        protected BinaryOp(Func<decimal, decimal, decimal> f, params string[] cmds) : base(cmds)
        {
            _func = f;
        }

        public override CommandActionEnum Run(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter as many numbers separated by lines or spaces. To end the add method, enter an empty line");
                var lists = new List<string>();
                var l = Console.ReadLine();
                while (!string.IsNullOrEmpty(l))
                {
                    lists.Add(l);
                    l = Console.ReadLine();
                }

                decimal sum = 0;
                foreach (var line in lists)
                {
                    sum += _op(line.Split(' '));
                }

                WriteLine(sum);
            }
            else
            {
                WriteLine(_op(args));
            }

            return CommandActionEnum.Continue;
        }

        private decimal _op(string[] args)
        {
            decimal result = 0;
            foreach (var a in args)
            {
                if (string.IsNullOrEmpty(a))
                    continue;

                decimal d;
                if (decimal.TryParse(a, out d))
                    result = _func(result, d);
                else
                    throw new Exception($"'{a}' could not be parsed");
            }
            return result;
        }
    }
}
