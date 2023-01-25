using System;
using System.Collections.Generic;
using System.Threading;

namespace AsyncAwait.SyncContext._05_ConfigureAwait
{
    internal class ConsoleMessageListenter
    {
        private static readonly LinkedList<ConsoleMessage> messagesList = new();

        public static void AddMessage(ConsoleMessage message)
        {
            messagesList.AddLast(message);
        }

        public void Listen()
        {
            while (true)
            {
                if (messagesList.Count > 0)
                {
                    ConsoleMessage message = messagesList.First.Value;

                    if (message != null)
                    {
                        messagesList.Remove(message);
                        DispatchMessage(message);
                    }
                }
            }
        }

        private void DispatchMessage(ConsoleMessage message)
        {
            SendOrPostCallback callback = message.Callback;
            object state = message.State;

            try
            {
                callback.Invoke(state);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An Exception was occured.");
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.WriteLine($"Exception Message: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
