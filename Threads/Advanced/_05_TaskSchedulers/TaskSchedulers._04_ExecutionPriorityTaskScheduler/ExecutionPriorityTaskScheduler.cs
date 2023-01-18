using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._04_ExecutionPriorityTaskScheduler
{
    internal class ExecutionPriorityTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> _tasksList = new LinkedList<Task>();

        public bool TryIncreasePriority(Task task)
        {
            lock (_tasksList)
            {
                var node = _tasksList.Find(task);

                if (node != null)
                {
                    _tasksList.Remove(node);
                    _tasksList.AddFirst(node);

                    return true;
                }
            }

            return false;
        }

        public bool TryDecreasePriority(Task task)
        {
            lock (_tasksList)
            {
                var node = _tasksList.Find(task);

                if (node != null)
                {
                    _tasksList.Remove(node);
                    _tasksList.AddLast(node);

                    return true;
                }
            }

            return false;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_tasksList)
            {
                return _tasksList;
            }
        }

        protected override void QueueTask(Task task)
        {
            lock (_tasksList)
            {
                _tasksList.AddLast(task);
            }

            ThreadPool.QueueUserWorkItem(ProcessNextQueuedItem, null);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) => TryExecuteTask(task);

        protected override bool TryDequeue(Task task)
        {
            lock (_tasksList)
            {
                return _tasksList.Remove(task);
            }
        }

        private void ProcessNextQueuedItem(object _)
        {
            Task task;

            lock (_tasksList)
            {
                if (_tasksList.Count > 0)
                {
                    task = _tasksList.First.Value;
                    _tasksList.RemoveFirst();
                }
                else
                {
                    return;
                }
            }

            TryExecuteTask(task);
        }
    }
}
