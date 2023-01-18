using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._03_LimitedConcurrencyTaskScheduler
{
    internal class LimitedConcurrencyTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> _tasksList = new();
        private readonly int _concurrencyLevel;

        private int _runningTasks = 0;

        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        public LimitedConcurrencyTaskScheduler()
            : this(1)
        {
        }

        public LimitedConcurrencyTaskScheduler(int concurrencyLevel)
        {
            if (concurrencyLevel < 1) throw new ArgumentOutOfRangeException(nameof(concurrencyLevel), $"Incorrect Value: {concurrencyLevel}");

            _concurrencyLevel = concurrencyLevel;
        }

        public override int MaximumConcurrencyLevel => _concurrencyLevel;

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

                if (_runningTasks < _concurrencyLevel)
                {
                    _runningTasks++;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (_currentThreadIsProcessingItems == false)
            {
                return false;
            }

            if (taskWasPreviouslyQueued == true)
            {
                TryDequeue(task);
            }

            return TryExecuteTask(task);
        }

        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                _currentThreadIsProcessingItems = true;

                try
                {
                    while (true)
                    {
                        Task task;

                        lock (_tasksList)
                        {
                            if (_tasksList.Count == 0)
                            {
                                _runningTasks--;
                                break;
                            }

                            task = _tasksList.First.Value;
                            _tasksList.RemoveFirst();
                        }

                        TryExecuteTask(task);
                    }
                }
                finally
                {
                    _currentThreadIsProcessingItems = false;
                }
            }, null);
        }

        protected override sealed bool TryDequeue(Task task)
        {
            lock (_tasksList)
            {
                return _tasksList.Remove(task);
            }
        }
    }
}
