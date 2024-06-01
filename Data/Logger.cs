using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Xml;

namespace Data
{
    internal class Logger
    {
        private static Logger _instance; // Singleton
        private static readonly object _lock = new object();

        private Queue<Action> queue = new Queue<Action>(); // przechowuje operacje logowania
        private AutoResetEvent hasNewItems = new AutoResetEvent(false); // informuje wątek logowania, kiedy pojawią się nowe elementy w kolejce
        private XmlWriter writer;
        private Thread loggingThread; // wątek do przetwarzania kolejki logowania
        private readonly int maxBufferSize = 100; // Maksymalna wielkość bufora

        private Logger()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true, // omija deklaracje xml
                Indent = true // includujemy wcięcia xml
            };
            writer = XmlWriter.Create("log.xml", settings);
            writer.WriteStartElement("Positions"); // element główny, w którym będziemy przechowywać pozycje
            loggingThread = new Thread(new ThreadStart(ProcessQueue));
            loggingThread.IsBackground = true; // będzie działał w tle czyli będzie automatycznie zakończony gdy reszta wątków zakończy działanie
            loggingThread.Start();
        }

        ~Logger()
        {
            loggingThread.Join(); // blokujemy wątek główny aplikacji do momentu, aż wątek liggingThread zakończy działanie, jest to potrzebne aby upewnić się że wątek logowania zakończy przetwarzanie wszystkich zadań w kolejce przed usunięciem Loggera
            lock (writer) // blokujemy żeby upewnić się że żadna kula nie będzie uzywać writera
            {
                writer.WriteEndElement(); // zamykamy główny element i kończymy writera
                writer.Close();
            }
        }

        public static Logger GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null) // używamy zamka żeby tylko jeden wątek mógł stworzyć nowy obiekt loggera, podwójnie sprawdzamy czy nie ma już loggera jako zabezpieczenie
                    {
                        _instance = new Logger();
                    }
                }
            }
            return _instance;
        }

        private void ProcessQueue()
        {
            while (true)
            {
                hasNewItems.WaitOne(); // wątek oczekuje na sygnał że w kolejce znajduje sięcoś do przetworzenia

                Queue<Action> queueCopy;
                lock (queue) // blokujemy kolejke aby zapobiec równoczesnemu dostepu do niej przez inne wątki
                {
                    queueCopy = new Queue<Action>(queue); // tworzymy kopię aby bezpiecznie iterować przez dane
                    queue.Clear(); // czyścimy żeby była gotowa do otrzymania nowych danych
                }

                foreach (Action log in queueCopy)
                {
                    log(); // wywołuje akcję z kolejki którą jest LogBallPositionAsXML()
                }
                hasNewItems.Reset(); // resetujemy obiekt wysyłający sygnał
            }
        }

        public void LogBallPosition(int id, Vector2 position)
        {
            lock (queue)
            {
                if (queue.Count >= maxBufferSize)
                {
                    Console.WriteLine("Buffer is full. Log entry ignored.");
                    return;
                }

                queue.Enqueue(() => LogBallPositionAsXML(id, position)); // jeśli bufor nie jest pełny dodajemy do niego akcję logowania ballPosition
            }

            hasNewItems.Set(); // wysyłamy sygnał że w buforze znajduje się akcja do przetworzenia
        }

        private void LogBallPositionAsXML(int id, Vector2 position)
        {
            lock (writer)
            {
                writer.WriteStartElement("Ball");
                writer.WriteElementString("ID", id.ToString());
                writer.WriteElementString("X", position.X.ToString());
                writer.WriteElementString("Y", position.Y.ToString());
                writer.WriteElementString("Timestamp", DateTime.Now.ToString("o"));
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}