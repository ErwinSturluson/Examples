using System;
using System.Threading;

namespace AsyncProgramming._03_ThreadPoolWrapper
{
    internal class ThreadPoolWrapper<TArg, TResult>
    {
        private readonly ManualResetEvent _lock = new(false);
        private readonly Func<TArg, TResult> _func;

        private Exception _exception;
        private TResult _result;
        private bool _completed;
        private bool _completedSuccessfully;

        public ThreadPoolWrapper(Func<TArg, TResult> func)
        {
            _func = func;
        }

        public bool Completed => _completed;

        public TResult Result
        {
            get
            {
                Wait();
                return _result;
            }
        }

        public bool CompletedSuccessfully => _completedSuccessfully;

        public void Start(TArg arg)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(MethodWrapper), arg);
        }

        public void Wait()
        {
            _lock.WaitOne();

            if (!_completedSuccessfully)
            {
                throw _exception;
            }
        }

        private void MethodWrapper(object state)
        {
            try
            {
                TArg arg = (TArg)state;

                _result = _func.Invoke(arg);

                _completedSuccessfully = true;
            }
            catch (Exception ex)
            {
                _completedSuccessfully = false;
                _exception = ex;
            }
            finally
            {
                _completed = true;
                _lock.Set();
            }
        }
    }
}
