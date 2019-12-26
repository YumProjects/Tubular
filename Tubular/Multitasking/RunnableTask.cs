using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tubular.Multitasking
{
    public abstract class RunnableTask
    {
        // True is the task is currently running
        public bool isRunning => _task != null && !_task.IsCompleted;

        // True if stop has been called
        protected bool stopSignaled { get; private set; }

        Task _task;

        // Starts the task
        public void Start(params object[] args)
        {
            if (isRunning) return;
            stopSignaled = false;
            OnStart();
            _task = Task.Run(() => Run(args));
        }

        // Signals the task to stop and waits for it
        public void Stop()
        {
            if (!isRunning) return;
            stopSignaled = true;
            while (isRunning) Thread.Sleep(1);
            OnEnd();
        }

        // Called before task starts
        protected abstract void OnStart();

        // Called after the task stops
        protected abstract void OnEnd();

        // Main run method
        protected abstract void Run(object[] args);
    }
}