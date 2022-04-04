using CrazyToys.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class LogGenerator : ILog
    {
        public async Task WriteExceptionToLog(string className, string methodName, Exception e)
        {
            DateTime timeOfError = DateTime.Now;
            string dateString = timeOfError.ToString();

            string createDir = @"ErrorLogs";
            // If directory does not exist, create it
            if (!Directory.Exists(createDir))
            {
                Directory.CreateDirectory(createDir);
            }

            string lines =
                $"Dato: {dateString}\n" +
                $"Fil: {className}\n" +
                $"Metode: {methodName}\n" +
                $"Exception: {e}\n" +
                $"ExceptionMessage: {e.Message}\n\n\n";

            using StreamWriter file = new(createDir + "/Log.txt", append: true);
            await file.WriteLineAsync(lines);
        }
    }
}
