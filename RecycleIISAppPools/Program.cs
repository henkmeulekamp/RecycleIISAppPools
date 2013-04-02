using System;
using System.Collections.Generic;
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
                Console.WriteLine("No apppools found!");
                return;
            }

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("No name requested to recycle on arg list, returning list off appools");

                appPools.ForEach(Console.WriteLine);
                WaitForKeyAndExit();
                return;
            }

            var names = args.ToList();

            var recycleList = NameFilter.Filter(names, appPools);

            if (!recycleList.Any())
            {             
                Console.WriteLine("No apppools found for argumentlist!");
                WaitForKeyAndExit();
                return;
            }

            recycleList.ToList().ForEach(recycler.Recycle);
            WaitForKeyAndExit();            
        }

        private static void WaitForKeyAndExit()
        {
            //auto kill after 5 seconds or wait for keyinput
            Task.WaitAny(new Task[] { new TaskFactory().StartNew(() => Console.ReadKey()) }, TimeSpan.FromSeconds(5));
        }
    }
}
