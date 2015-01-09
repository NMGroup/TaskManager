using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mephi.Cybernetics.Nm.TaskManager
{
    enum State
    {
        Free,
        InProcess,
        Done
    }
    class ThreadForTM
    {
        private static int counter = 0;
        private Thread _thread;
        private State _state;

        public TaskRE Task { get; private set; }
        public int ID { get; private set; }

        public State State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
                if(_state == State.Done)
                    StateIsChanged(this);
            }
        }

        public delegate void ChangedState(ThreadForTM thread);

        public event ChangedState StateIsChanged;

        public ThreadForTM()
        {
            ID = counter;
            ++counter;
        }

        public void Invoke(TaskRE task)
        {
            /*var mre = new ManualResetEvent(false);
            mre.WaitOne();

            // полезная логика

            mre.Reset();


            // манагере:
            mre.Set();*/

            State = State.Free;
            Task = task;
            _thread = new Thread(() =>
            {
                this.State = State.InProcess;
                Task.ResultValue.Value = Task.Func.FuncDelegate.DynamicInvoke(Task.Arguments);
                this.State = State.Done;

            });
            
            _thread.Start();
            
            
        }
    }
}
