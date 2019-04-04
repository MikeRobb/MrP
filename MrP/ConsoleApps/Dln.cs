namespace MrP.ConsoleApps
{
    public class Dln : AConsoleImplementation
    {
        public Dln() : base("dln")
        {
        }

        public override CommandActionEnum Run(string[] args)
        {
            WriteLine("Running base module");
            WriteLine(string.Join(' ', args));
            return CommandActionEnum.Continue;
        }
    }

    public class DlnPlayer : AConsoleImplementation
    {
        public DlnPlayer() : base("dln player", "dln players")
        {
        }

        public override CommandActionEnum Run(string[] args)
        {
            WriteLine("Running Player module");
            WriteLine(string.Join(' ', args));
            return CommandActionEnum.Continue;
        }
    }
}
