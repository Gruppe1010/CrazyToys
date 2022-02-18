using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ILog
    {

        Task WriteExceptionToLog(string className, string methodName, Exception e);

    }
}
