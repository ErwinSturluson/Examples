using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulers._06_SynchronousTaskScheduler
{
    internal class SynchronousTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> _tasksList = new();

        internal int ExecuteTasksDelayMilliseconds { get; set; } = 0;

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            Console.WriteLine($"Task List was requested in Thread#{Environment.CurrentManagedThreadId}.");
            return _tasksList;
        }

        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"Task with Id#{task.Id} was queued in Thread#{Environment.CurrentManagedThreadId}.");

            lock (_tasksList)
            {
                _tasksList.AddLast(task);
            }

            ExecuteTasks();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine($"Task with Id#{task.Id} was tried execute inline in Thread#{Environment.CurrentManagedThreadId}.");

            lock (_tasksList)
            {
                _tasksList.Remove(task);
            }

            bool executedInline = TryExecuteTask(task);

            if (executedInline)
            {
                Console.WriteLine($"Task with Id#{task.Id} was successfully executed inline in Thread#{Environment.CurrentManagedThreadId}.");
            }
            else
            {
                Console.WriteLine($"Task with Id#{task.Id} was failed to execute inline in Thread#{Environment.CurrentManagedThreadId}.");
            }

            return executedInline;
        }

        protected override bool TryDequeue(Task task)
        {
            Console.WriteLine($"Task with Id#{task.Id} was tried to dequeue in Thread#{Environment.CurrentManagedThreadId}.");

            bool taskDequeued = false;

            lock (_tasksList)
            {
                taskDequeued = _tasksList.Remove(task);
            }

            if (taskDequeued)
            {
                Console.WriteLine($"Task with Id#{task.Id} was dequeued successfully in Thread#{Environment.CurrentManagedThreadId}.");
            }
            else
            {
                Console.WriteLine($"Task with Id#{task.Id} was failed to dequeue in Thread#{Environment.CurrentManagedThreadId}.");
            }

            return taskDequeued;
        }

        private void ExecuteTasks()
        {
            while (true)
            {
                Thread.Sleep(ExecuteTasksDelayMilliseconds);
                Task task = null;

                lock (_tasksList)
                {
                    if (_tasksList.Count == 0)
                    {
                        break;
                    }

                    task = _tasksList.First.Value;
                    _tasksList.RemoveFirst();
                }

                if (task == null)
                {
                    break;
                }

                TryExecuteTask(task);
            }
        }
    }
}
