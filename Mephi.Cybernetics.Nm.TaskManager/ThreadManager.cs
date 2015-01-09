﻿using System;
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
        private ObservableCollection<TaskRE> _currentTaskList;

        private ObservableCollection<ThreadForTM> _busyThreads;
        private ObservableCollection<ThreadForTM> _freeThreads;

        const int quantityOfThreads = 50;

        public ObservableCollection<TaskRE> CurrentTaskList
        {
            get { return _currentTaskList; }
        }

        #region Ctor



        public ThreadManager()
        {
            _currentTaskList = new ObservableCollection<TaskRE>();
            _busyThreads = new ObservableCollection<ThreadForTM>();
            _freeThreads = new ObservableCollection<ThreadForTM>();   

            for (int i = 0; i < quantityOfThreads; ++i)
            {
                var NewThread = new ThreadForTM();
                NewThread.StateIsChanged += OnStateThreadForTMCHanged;
                _freeThreads.Add(NewThread);
            }
            
        }

        #endregion

        public bool TryGetThread( TaskRE task )
        {
            lock (this)
            {
                if (_freeThreads.Count != 0)
                {
                    ThreadForTM freeThread = _freeThreads[0];
                    _currentTaskList.Add(task);
                    _busyThreads.Add(freeThread);
                    _freeThreads.Remove(freeThread);
                    freeThread.Invoke(task);
                    return true;
                }
            }
            return false;
        }

        private void OnStateThreadForTMCHanged(ThreadForTM e)
        {
            lock (e)
            {
                if (e.State == State.Done)
                {
                    _freeThreads.Add(e);
                    _busyThreads.Remove(e);
                    _currentTaskList.Remove(e.Task);
                    Console.WriteLine("я готов");
                }
            }

        }

    }
}
 