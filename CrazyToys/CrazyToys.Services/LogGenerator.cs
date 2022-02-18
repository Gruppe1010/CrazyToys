using CrazyToys.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CrazyToys.Web.Logging
{
    public class LogGenerator : ILog
    {
        public async Task WriteExceptionToLog(string className, string methodName,  Exception e)
        {
            DateTime timeOfError = DateTime.Now;
            string dateString = timeOfError.ToString();

            string lines =
                $"Dato: {dateString}\n" +
                $"Fil: {className}\n" +
                $"Metode: {methodName}\n" +
                $"Exception: {e}\n" +
                $"ExceptionMessage: {e.Message}\n\n\n";

            using StreamWriter file = new("../CrazyToys/OurLogs/Log.txt", append: true);
            await file.WriteLineAsync(lines);
        }
    }
}
