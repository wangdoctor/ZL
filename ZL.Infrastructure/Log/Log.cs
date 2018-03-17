using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZL.Infrastructure
{
   public class Log
    {
        public void Info(string v)
        {
            Logger logger = LogManager.GetLogger("SimpleDemo");
            logger.Info(v);
        }
        public void Error(string v)
        {
            Logger logger = LogManager.GetLogger("SimpleDemo");
            logger.Error(v);
        }
    }
}
