using System;
using System.Linq;
using System.Threading.Tasks;
using CmdLine;

namespace RecycleIISAppPools
{
    class Program
    {
        static void Main(string[] args)
        {
            RecycleCommands arguments = null;
            try
            {
                 arguments = CommandLine.Parse<RecycleCommands>();
            }
            catch (CommandLineException exception)
            {
                ShowHelp(exception);
                return;
            }

            if (arguments != null && arguments.Help)
            {
                ShowHelp();
                return;
            }

            if(arguments==null) arguments = new RecycleCommands();

            if(!string.IsNullOrEmpty(arguments.Server)) Console.WriteLine("Server : {0}",arguments.Server);
            if (!string.IsNullOrEmpty(arguments.Filter)) Console.WriteLine("Filter : {0}", arguments.Filter);


            var recycler = new Recycler(arguments.Server, arguments.Username, arguments.Password);
            var appPools = recycler.GetAppPools();
           
            if (!appPools.Any())
            {
                Console.WriteLine("No app pools found!");
                return;
            }

            if (string.IsNullOrEmpty(arguments.Filter))
            {
                Console.WriteLine("No name requested to recycle on arg list, returning list off appools.");
                Console.WriteLine("Application pool names:");
                appPools.ForEach(Console.WriteLine);
                Console.WriteLine("---");
                Console.WriteLine("Filter by name 'recycle /f name' or 'recycle /f name*' or  'recycle /f name1* name2 name13'");
                Console.WriteLine("Multiple names allowed (with space as divider) and * is wilcard at start and/or end of name");

                WaitForKeyAndExit();
                return;
            }

            var names = arguments.Filter.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries).ToList();

            var recycleList = NameFilter.Filter(names, appPools).ToList();

            if (!recycleList.Any())
            {             
                Console.WriteLine("No app pools found for argumentlist!");
                WaitForKeyAndExit();
                return;
            }

            recycleList.ForEach(recycler.Recycle);
            WaitForKeyAndExit();            
        }

        private static void ShowHelp(CommandLineException exception = null)
        {
            if (exception != null)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));    
            }
            else
            {
                var help = new CommandArgumentHelp(typeof (RecycleCommands));
                Console.WriteLine(help.GetHelpText(Console.BufferWidth));
            }
        }
    

        private static void WaitForKeyAndExit()
        {
            //auto kill after 3 seconds or wait for keyinput
            Task.WaitAny(new Task[] { new TaskFactory().StartNew(() => Console.ReadKey()) }, TimeSpan.FromSeconds(3));
        }
    }


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
