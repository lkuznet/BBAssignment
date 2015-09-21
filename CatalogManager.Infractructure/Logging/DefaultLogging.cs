using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Infractructure.Logging
{
    public class DefaultLogging:ILog
    {
        public void LogError(string errorMessage)
        {
            Debug.WriteLine(errorMessage);
        }
    }
}
