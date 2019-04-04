namespace MrP.ConsoleApps
{
    public class Exit : AConsoleImplementation
    {
        public Exit() : base("exit", "quit", "")
        {
        }

        public override CommandActionEnum Run(string[] args)
        {
            return CommandActionEnum.Exit;
        }
    }
    public class Crash : AConsoleImplementation
    {
        public Crash() : base("crash")
        {
        }

        public override CommandActionEnum Run(string[] args)
        {
            return CommandActionEnum.Crash;
        }
    }
}
