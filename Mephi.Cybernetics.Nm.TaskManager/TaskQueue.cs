using System;
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
        private ObservableCollection<TaskRE> _pendingTaskList;
        /// <summary> Выполненые задания </summary>
        private ObservableCollection<TaskRE> _completedTaskList;

        private ThreadManager _threadManager;

        public bool TryGetTask(out TaskRE task)
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

        public void AddTask(TaskRE task)
        {
            //if (!IsTaskDone(task))
            lock (_pendingTaskList)
            {
                _pendingTaskList.Add(task);
            }

        }

        public void OnPendingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TaskRE t = null;
            if (TryGetTask(out t))
                _threadManager.TryGetThread(t);
        }

        public void OnCompletedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TaskRE t = null;
            if (TryGetTask(out t))
                _threadManager.TryGetThread(t);
        }


        public TaskQueue()
        {
            _pendingTaskList = new ObservableCollection<TaskRE>();
            _completedTaskList = new ObservableCollection<TaskRE>();
            _threadManager = new ThreadManager();

            _pendingTaskList.CollectionChanged += this.OnPendingCollectionChanged;
            _completedTaskList.CollectionChanged += this.OnCompletedCollectionChanged;
            _threadManager.CurrentTaskList.CollectionChanged += CurrentTaskListOnCollectionChanged;
        }

        private void CurrentTaskListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (_completedTaskList)
            {
                if (e.OldItems != null)
                {
                    foreach (TaskRE task in e.OldItems)
                    {
                        _completedTaskList.Add(task);
                    }
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
