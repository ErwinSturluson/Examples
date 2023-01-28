using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncOperations._04_IO.NetCore31
{
    internal class Program
    {
        private static readonly object _consoleLock = new object();
        private static HttpClient _httpClient;

        private static void Main(string[] args)
        {
            _httpClient = new HttpClient();

            StartThreadPoolMonitoring();

            HttpResponseMessage asyncResult = SendHttpRequestAsync().Result;

            lock (_consoleLock)
            {
                Console.WriteLine("Async Result was received in a calling method.");
                Console.WriteLine("Press any key to finish the program.");
            }

            Console.ReadKey();
        }

        private static async Task<HttpResponseMessage> SendHttpRequestAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();

            HttpResponseMessage asyncResult = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://visualstudio.com/"));

            sw.Stop();

            Thread.Sleep(200);

            lock (_consoleLock)
            {
                Console.WriteLine($"Continuation of Async IO Result of HTTP Request is executing in ThreeadPoolThread: [{Thread.CurrentThread.IsThreadPoolThread}].");
                Console.WriteLine("Async IO Result of HTTP Request was Received after" + MakeElapsedTimeString(sw.Elapsed));
            }

            Thread.Sleep(200);

            return asyncResult;
        }

        private static string MakeElapsedTimeString(TimeSpan elapsed) => $"{Environment.NewLine}ElapsedMilliseconds:{elapsed.TotalMilliseconds}";

        private static void StartThreadPoolMonitoring()
        {
            Thread thredPoolMonitoringThread = new Thread(() =>
            {
                while (true)
                {
                    ThreadPool.GetAvailableThreads(out int workerThreads, out int iocpThreads);
                    ThreadPool.GetMaxThreads(out int workerMaxThreads, out int iocpMaxThreads);

                    bool usedWorkerThreads = workerThreads < workerMaxThreads;
                    bool usedIocpThreads = iocpThreads < iocpMaxThreads;

                    if (usedWorkerThreads || usedIocpThreads)
                    {
                        lock (_consoleLock)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.Write("Worker Threads[");

                            if (usedWorkerThreads)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.Write(workerThreads);
                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.Write(" of ");

                            if (usedWorkerThreads)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            }
                            Console.Write(workerMaxThreads);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("] ");

                            Console.Write("IOCP Threads[");

                            if (usedIocpThreads)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.Write(iocpThreads);
                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.Write(" of ");

                            if (usedIocpThreads)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            }
                            Console.Write(iocpMaxThreads);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("] ");

                            Console.ResetColor();
                            Console.WriteLine();
                        }

                        Thread.Sleep(100);
                    }
                }
            });

            thredPoolMonitoringThread.IsBackground = true;

            thredPoolMonitoringThread.Start();
        }
    }
}
