using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace RecycleIISAppPools
{
    public class Recycler
    {
        public List<string> GetAppPools()
        {
            try
            {
                var wsvc = new DirectoryEntry("IIS://LocalHost/w3svc", "", "");

                var list = new List<string>();

                foreach (DirectoryEntry site in wsvc.Children.Cast<DirectoryEntry>().Where(site => site.Name.Equals("AppPools", StringComparison.OrdinalIgnoreCase)))
                {
                    list.AddRange(from DirectoryEntry child in site.Children select child.Name);
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed enumerating appools. Either IIS is not installed or application needs to run in amdinistrator mode.\n{0}",e);
                return new List<string>(0);
            }
           
        }

        public void Recycle(string appPool)
        {
            Console.WriteLine("Recycling {0}", appPool);
          
            try
            {
                string appPoolPath = "IIS://localhost/W3SVC/AppPools/" + appPool;

                using (var appPoolEntry = new DirectoryEntry(appPoolPath))
                {
                    appPoolEntry.Invoke("Recycle", null);
                    appPoolEntry.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed Recycling {0}:\n{1}", appPool, e);                
            }
            Console.WriteLine("Finished Recycling {0}", appPool);
        }
    }
}