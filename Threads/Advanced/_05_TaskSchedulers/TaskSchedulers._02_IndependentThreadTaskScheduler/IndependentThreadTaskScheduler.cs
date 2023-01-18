using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._02_IndependentThreadTaskScheduler
{
    internal class IndependentThreadTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task)
        {
            Thread taskThread = new(() => TryExecuteTask(task));

            taskThread.IsBackground = true;

            taskThread.Start();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => TryExecuteTask(task);
    }
}
