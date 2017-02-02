using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInsightsMVCFilters
{
    public class GlobalConfiguration
    {
        public bool LogPayload { get; set; }
        private static GlobalConfiguration _instance = null;
        private static readonly object Padlock = new object();
        private GlobalConfiguration()
        {
            
        }

        public static GlobalConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalConfiguration();
                        }
                    }
                }

                return _instance;
            }

        }


    }
}
