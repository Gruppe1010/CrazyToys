using System;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ILog
    {

        Task WriteExceptionToLog(string className, string methodName, Exception e);

    }
}
