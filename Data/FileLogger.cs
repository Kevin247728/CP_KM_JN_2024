using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public interface ILogger
    {
        void Log(string message, int priority = 1);
        void FlushLogs(object state);
    }

    public class LogEntry : IComparable<LogEntry>
    {
        public DateTime Timestamp { get; }
        public string Message { get; }
        public int Priority { get; }

        public LogEntry(string message, int priority)
        {
            Timestamp = DateTime.Now;
            Message = message;
            Priority = priority;
        }

        public int CompareTo(LogEntry other)
        {
            //higher priority come first
            int priorityComparison = other.Priority.CompareTo(Priority);
            return priorityComparison != 0 ? priorityComparison : Timestamp.CompareTo(other.Timestamp);
        }

        public override string ToString()
        {
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Priority}] {Message}";
        }
    }

    public class CustomLogger : ILogger, IDisposable
    {
        private readonly string _logFilePath;
        private readonly PriorityQueue<LogEntry> _logQueue;
        private readonly object _lockObject;
        private readonly Timer _flushTimer;

        public CustomLogger()
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            _logFilePath = Path.Combine(baseDirectory, "log.txt");
            _logQueue = new PriorityQueue<LogEntry>();
            _lockObject = new object();

            // Flush log to file every 2 seconds
            _flushTimer = new Timer(FlushLogs, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));

            AppDomain.CurrentDomain.ProcessExit += (sender, e) => Dispose();
            AppDomain.CurrentDomain.DomainUnload += (sender, e) => Dispose();
        }

        public void Log(string message, int priority = 1)
        {
            var logEntry = new LogEntry(message, priority);

            lock (_lockObject)
            {
                _logQueue.Enqueue(logEntry);
            }
        }

        public void FlushLogs(object state)
        {
            List<LogEntry> entriesToWrite = new List<LogEntry>();

            lock (_lockObject)
            {
                while (_logQueue.Count > 0)
                {
                    entriesToWrite.Add(_logQueue.Dequeue());
                }
            }

            if (entriesToWrite.Count > 0)
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    foreach (var entry in entriesToWrite)
                    {
                        writer.WriteLine(entry.ToString());
                    }
                }
            }
        }

        public void Dispose()
        {
            _flushTimer.Dispose();
            FlushLogs(null);
        }
    }

    //implementation of a PriorityQueue using a SortedSet.
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly SortedSet<T> _set = new SortedSet<T>();

        public int Count => _set.Count;

        public void Enqueue(T item)
        {
            _set.Add(item);
        }

        public T Dequeue()
        {
            if (_set.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            T item = _set.Min;
            _set.Remove(item);
            return item;
        }
    }
}
