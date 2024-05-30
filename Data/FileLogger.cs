using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public interface ILogger
    {
        Task Log(BallJsonModel ball);
        Task LogMessage(string message);
    }

    public class FileLogger : ILogger
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        public async Task Log(BallJsonModel ball)
        {
            await _semaphore.WaitAsync();
            try
            {
                using (StreamWriter sw = File.AppendText(Directory.GetCurrentDirectory() + $"\\log.json"))
                {
                    await sw.WriteLineAsync(JsonSerializer.Serialize(ball));
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task LogMessage(string message)
        {
            await _semaphore.WaitAsync();
            try
            {
                DateTime utcNow = DateTime.Now;
                string formattedTime = utcNow.ToString("yyyy-MM-dd HH:mm:ss.fffZ");
                string logMessage = $"{formattedTime:s} {message}";
                using (StreamWriter sw = File.AppendText(Directory.GetCurrentDirectory() + $"\\log.json"))
                {
                    await sw.WriteLineAsync(logMessage);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
