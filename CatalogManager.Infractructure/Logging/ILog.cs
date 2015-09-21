using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogManager.Infractructure.Logging

{
    public interface ILog
    {
        void LogError(string errorMessage);
    }
}
