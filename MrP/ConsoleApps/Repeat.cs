namespace MrP.ConsoleApps
{
    public class Repeat : AConsoleImplementation
    {
        public Repeat() : base("repeat", "rp")
        {
        }

        public override CommandActionEnum Run(string[] args)
        {
            foreach (var a in args)
            {
                WriteLine(a);
            }
            return CommandActionEnum.Continue;
        }
    }
}
