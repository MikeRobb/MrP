using System;

namespace MrP.ConsoleApps
{
    public class Advancing : AConsoleImplementation
    {
        public int Num;

        public Advancing() : base("advancing")
        {
            Num = 0;
        }

        public override CommandActionEnum Run(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("reset", StringComparison.CurrentCultureIgnoreCase))
                    Num = 0;
            }

            WriteLine(Num);
            Num++;
            return CommandActionEnum.Continue;
        }
    }
}
