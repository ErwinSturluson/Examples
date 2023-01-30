using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeCancellation._02_CancellationToken.TaskScheduler
{
    internal class DelayTaskScheduler : System.Threading.Tasks.TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task)
        {
            new Thread(() =>
            {
                Thread.Sleep(500);

                if (!task.IsCanceled)
                {
                    Thread taskThread = new(() => TryExecuteTask(task));

                    taskThread.IsBackground = true;

                    taskThread.Start();
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => TryExecuteTask(task);
    }
}
