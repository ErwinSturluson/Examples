using System.Threading;

namespace AsyncAwait.SyncContext._05_ConfigureAwait
{
    internal class ConsoleMessage
    {
        public SendOrPostCallback Callback { get; set; }

        public object State { get; set; }

        public ConsoleMessage()
        { }

        public ConsoleMessage(SendOrPostCallback callback, object state)
        {
            Callback = callback;
            State = state;
        }
    }
}
