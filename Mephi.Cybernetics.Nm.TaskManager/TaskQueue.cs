using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.ObjectModel;

namespace Mephi.Cybernetics.Nm.TaskManager
{
    public class TaskQueue
    {
        /// <summary> Невыполненые задания </summary>
        private ConcurrentQueue<TaskRegistryEntry> _pendingTaskList;
        /// <summary> Выполненые задания </summary>
        private ConcurrentQueue<TaskRegistryEntry> _completedTaskList;

        private ThreadManager _threadManager;

        public bool TryGetTask(out TaskRegistryEntry task)
        {
            lock (_pendingTaskList)
            {
                if (_pendingTaskList.Count != 0)
                {
                    var popedTask = _pendingTaskList[0];
                    _pendingTaskList.RemoveAt(0);
                    task = popedTask;
                    return true;
                }
                else
                {
                    task = null;
                    return false;
                }
            }
        }

        public void AddTask(TaskRegistryEntry task)
        {
            lock (_pendingTaskList)
            {
                _pendingTaskList.Add(task);
            }
            TaskRegistryEntry t = null;
            if (TryGetTask(out t))
                _threadManager.TryGetThread(t);
        }

        private void AddToComplete(TaskRegistryEntry task)
        {
            lock(_completedTaskList)
            _completedTaskList.Add(task);

            TaskRegistryEntry t = null;
            if (TryGetTask(out t))
                _threadManager.TryGetThread(t);
        }


        public TaskQueue()
        {
            _pendingTaskList = new ObservableCollection<TaskRegistryEntry>();
            _completedTaskList = new ObservableCollection<TaskRegistryEntry>();
            _threadManager = new ThreadManager();

            _threadManager.CurrentTaskList.CollectionChanged += CurrentTaskListOnCollectionChanged;
        }

        private void CurrentTaskListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TaskRegistryEntry task in e.OldItems)
                {
                    AddToComplete(task);
                }
            }
            
        }

        /*public bool IsTaskDone(TaskRE task)
        {
            foreach (var taskRE in _completedTaskList)
            {
                if ((task.Name == taskRE.Name))
                {
                    for (int i = 0; i < task.Arguments.Length; i++)
                    {
                        if (task.Arguments[i] == taskRE.Arguments[i])
                        {
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //Console.WriteLine("here");
                    task.ResultValue = taskRE.ResultValue.Copy();
                    return true;
                }
            }
            return false;
        }*/

    }
}
