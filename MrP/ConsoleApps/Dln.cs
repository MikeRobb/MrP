using System;

namespace MrP.ConsoleApps
{
    public class Dln : AConsoleImplementation
    {
        private string PlayerFile;

        public Dln() : base("dln")
        {
        }

        public override CommandActionEnum Run(string[] args)
        {
            

            return CommandActionEnum.Continue;
        }

        private CommandActionEnum Players(string[] args)
        {

        }
    }
}
