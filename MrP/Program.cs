using System;

namespace MrP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Constructing ConsoleController...");
            var cc = new ConsoleController();

            Console.WriteLine("Running ConsoleController...");
            cc.Run();

            Console.WriteLine("Exited ConsoleController");
            Console.ReadLine();
        }
    }
}
