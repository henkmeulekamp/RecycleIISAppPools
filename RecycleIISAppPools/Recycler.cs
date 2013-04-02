using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace RecycleIISAppPools
{
    public class Recycler
    {
        private readonly string _serverName;
        private readonly string _userName;
        private readonly string _password;

        public Recycler(string serverName, string userName, string password)
        {
            _serverName = serverName;
            _userName = userName;
            _password = password;
        }

        public List<string> GetAppPools()
        {
            try
            {
                var wsvc = new DirectoryEntry(string.Format("IIS://{0}/w3svc", _serverName), _userName, _password);

                var list = new List<string>();

                foreach (DirectoryEntry site in wsvc.Children.Cast<DirectoryEntry>().Where(site => site.Name.Equals("AppPools", StringComparison.OrdinalIgnoreCase)))
                {
                    list.AddRange(from DirectoryEntry child in site.Children select child.Name);
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed enumerating app pools. Either IIS is not installed or application needs to run in administrator mode.\n{0}",e);
                return new List<string>(0);
            }
           
        }

        public void Recycle(string appPool)
        {
            Console.WriteLine("Recycling {0}", appPool);
          
            try
            {
                string appPoolPath = string.Format("IIS://{0}/W3SVC/AppPools/{1}",_serverName, appPool);

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