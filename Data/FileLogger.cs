using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Data
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class FileLogger : ILogger
    {
        private readonly Serilog.Core.Logger _logger;

        public FileLogger() //dopracowac miejsce zapisywania pliku z logami bo aktualnie jest to View/bin/Debug
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            string logFilePath = Path.Combine(baseDirectory, "log.txt");

            _logger = new LoggerConfiguration()
                .WriteTo.File(logFilePath)
                .CreateLogger();
        }

        public void Log(string message)
        {
            _logger.Information(message);
        }
    }
}

