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



        public TaskRegistryEntry Task { get; private set; }
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

        public void Invoke(TaskRegistryEntry task)
        {
            /*var mre = new ManualResetEvent(false);
            mre.WaitOne();

            // полезная логика

            mre.EmptyTask();


            // манагере:
            mre.Set();*/


            State = State.Free;
            Task = task;
            _thread = new Thread(
            () =>
                {
                    this.State = State.InProcess;
                    Task.ResultValue.Value = Task.Func.FuncDelegate.DynamicInvoke(Task.Arguments);
                    this.State = State.Done;

                }
            );
            _thread.Start();

        }
    }

    public class IkThreadManager
    {
        private readonly Thread[] _threads;
        private readonly OptionalValue<TaskRegistryEntry>[] _data;
        private readonly bool[] _stopFlags;

        public event EventHandler ThreadAvailable;

        public IkThreadManager(int threadCount)
        {
            _data = new OptionalValue<TaskRegistryEntry>[threadCount];
            for (var i = 0; i < threadCount; i++)
                _data[i] = new OptionalValue<TaskRegistryEntry>();

            _threads = new Thread[threadCount];
            for (var i = 0; i < threadCount; i++)
            {
                _threads[i] = new Thread(id => ThreadWorker((int)id));
            }

            _stopFlags = new bool[threadCount];
        }

        private void ThreadWorker(int id)
        {
            while (true)
            {
                var task = _data[id];
                task.Wait();
                if (task.ShouldStop) return;
                
                if (!task.IsEmpty)
                {
                    task.Value.Func.FuncDelegate.DynamicInvoke(task.Value.Arguments);
                    task.EmptyTask();
                    if (ThreadAvailable != null)
                        ThreadAvailable.BeginInvoke(this, new EventArgs(), null, null);
                    // throw event that there is an empty thread
                }

            }
        }

        public void Start()
        {
            foreach (var t in _threads)
            {
                t.Start();
            }
        }

        public void Stop()
        {
            for (var i = 0; i < _threads.Length; i++)
            {
                _data[i].ShouldStop = true;
//                _threads[i].Abort();
            }
        }
    }

    internal class OptionalValue<T>
    {
        private T _value = default(T);
        private bool _isEmpty = true;
        private readonly ManualResetEvent _manualResetEvent;
        private bool _shouldStop = false;

        public OptionalValue()
        {
            _manualResetEvent = new ManualResetEvent(false);
        }

        public OptionalValue(T value) : this()
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                if (_isEmpty)
                    throw new ArgumentException("Value is not given");
                else
                    return _value;
            }
            set
            {
                _value = value;
                _isEmpty = false;
                _manualResetEvent.Set();
            }
        }

        public bool IsEmpty
        {
            get { return _isEmpty; }
        }

        public bool ShouldStop
        {
            get { return _shouldStop; }
            set
            {
                _shouldStop = true;
                _manualResetEvent.Set();
            }
        }

        public void EmptyTask()
        {
            _isEmpty = true;
            _manualResetEvent.Reset();
        }

        public void Wait()
        {
            _manualResetEvent.WaitOne();
        }
    }
}
