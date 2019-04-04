using System;

namespace MrP
{
    class Program
    {
        static void Main(string[] args)
        {
            var cc = new ConsoleController();
            Console.WriteLine("Ready");

            CommandActionEnum curAction = cc.Run(Console.ReadLine());
            while (curAction == CommandActionEnum.Continue)
            {
                curAction = cc.Run(Console.ReadLine());
            }

            if (curAction == CommandActionEnum.Crash)
                Console.Error.WriteLine("Crash!");

            Console.Write("Exiting...");
            Console.ReadLine();
        }
    }
}
