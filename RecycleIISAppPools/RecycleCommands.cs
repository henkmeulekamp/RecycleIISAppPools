using CmdLine;

namespace RecycleIISAppPools
{
    [CommandLineArguments(Program = "Recycle", Title = "IIS App Pool recycler", Description = "Recycle IIS App Pools from command line. Questions:henk@meulekamp.net")]
    public class RecycleCommands
    {
        [CommandLineParameter(Command = "?", Default = false, Description = "Show Help", Name = "Help", IsHelp = true)]
        public bool Help { get; set; }

        [CommandLineParameter(Name = "filter", Command = "f", Required = false, Description = "App Pool name filters, use keyword separated with spaces, Wildcard = *")]
        public string Filter { get; set; }

        [CommandLineParameter(Name = "server", Command = "s", Default = "localhost", Description = "Specifies remote server name, leave empty for localhost")]
        public string Server { get; set; }

        [CommandLineParameter(Name = "username", Command = "u", Default = "", Description = "Specifies username for server, leave empty when using current account")]
        public string Username { get; set; }

        [CommandLineParameter(Name = "password", Command = "p", Default = null, Description = "Specifies password for server, leave empty when using current accountt")]
        public string Password { get; set; }    
    }
}