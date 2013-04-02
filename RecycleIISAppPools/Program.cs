using System;
using System.Linq;
using System.Threading.Tasks;

namespace RecycleIISAppPools
{
    class Program
    {
        static void Main(string[] args)
        {
            var recycler = new Recycler();
            var appPools = recycler.GetAppPools();
           
            if (!appPools.Any())
            {
                Console.WriteLine("No app pools found!");
                return;
            }

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("No name requested to recycle on arg list, returning list off appools.");
                Console.WriteLine("Application pool names:");
                appPools.ForEach(Console.WriteLine);
                Console.WriteLine("---");
                Console.WriteLine("Filter by name 'recycle name' or 'recycle name*' or  'recycle name1* name2 name13'");
                Console.WriteLine("Multiple names allowed (with space as divider) and * is wilcard at start and/or end of name");

                WaitForKeyAndExit();
                return;
            }

            var names = args.ToList();

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

        private static void WaitForKeyAndExit()
        {
            //auto kill after 3 seconds or wait for keyinput
            Task.WaitAny(new Task[] { new TaskFactory().StartNew(() => Console.ReadKey()) }, TimeSpan.FromSeconds(3));
        }
    }
}
