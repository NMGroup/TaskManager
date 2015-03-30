using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Mephi.Cybernetics.Nm.TaskManager
{
    class ThreadManager
    {
        private ObservableCollection<TaskRegistryEntry> _currentTaskList;

        private BlockingCollection<ThreadForTM> _busyThreads;
        private BlockingCollection<ThreadForTM> _freeThreads;

        private ManualResetEvent _busyThreadManagerIndicator = new ManualResetEvent(true);

        const int quantityOfThreads = 50;

        public ObservableCollection<TaskRegistryEntry> CurrentTaskList
        {
            get
            {
                return _currentTaskList;
            }
        }

        #region Ctor

        public ThreadManager()
        {
            _currentTaskList = new ObservableCollection<TaskRegistryEntry>();
            _busyThreads = new BlockingCollection<ThreadForTM>();
            _freeThreads = new BlockingCollection<ThreadForTM>();   

            for (int i = 0; i < quantityOfThreads; ++i)
            {
                var NewThread = new ThreadForTM();
                NewThread.StateIsChanged += OnStateThreadForTMCHanged;
                _freeThreads.Add(NewThread);
            }
            
        }

        #endregion

        public bool TryGetThread( TaskRegistryEntry task )
        {
            lock (_freeThreads)
            {
                if (_freeThreads.Count != 0)
                {
                    ThreadForTM freeThread = _freeThreads[0];
                    _currentTaskList.Add(task);
                    _freeThreads.TryTake(out freeThread);
                    lock (_busyThreads)
                    {
                        _busyThreads.Add(freeThread);
                    }
                    freeThread.Invoke(task);
                    return true;
                }
            }
            return false;       
        }

        private object syncThreadAccess = new object();

        private void OnStateThreadForTMCHanged(ThreadForTM e)
        {
            lock (_busyThreads)
                lock (_freeThreads)
            {
                if (e.State == State.Done)
                {
                    _busyThreads.Remove(e);
                    _freeThreads.Add(e);
                    _currentTaskList.Remove(e.Task);
                }

            }
        }

    }
}
 