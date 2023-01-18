using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._05_SynchronizationContextTaskScheduler
{
    internal class SynchronizationContextTaskScheduler : TaskScheduler
    {
        private readonly SynchronizationContext _synchronizationContext;

        public SynchronizationContextTaskScheduler()
            : this(SynchronizationContext.Current)
        {
        }

        public SynchronizationContextTaskScheduler(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
        }

        protected override IEnumerable<Task> GetScheduledTasks() => Enumerable.Empty<Task>();

        protected override void QueueTask(Task task) => _synchronizationContext.Post(_ => TryExecuteTask(task), null);

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            bool contextsEqual = _synchronizationContext == SynchronizationContext.Current;

            return contextsEqual && TryExecuteTask(task);
        }
    }
}
