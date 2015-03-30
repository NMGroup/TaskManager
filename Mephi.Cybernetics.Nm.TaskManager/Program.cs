using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mephi.Cybernetics.Nm.TaskManager
{
    class Program
    {

        static void Main(string[] args)
        {
            int p = 1;
            int o = 1;
            TaskQueue tq = new TaskQueue();
            TaskRegistryEntry zTaskRe = new TaskRegistryEntry(name:String.Format("task1"), arguments: new object[]{p,o}, 
            dlgt: new Func<int,int, int>((int k,int l) => 
            {

                return k+l;
            }));
            Console.WriteLine("end");
            TaskRegistryEntry checkAction = new TaskRegistryEntry(
               name: String.Format("taskAction"),
               arguments: new object[]{p},
               dlgt: new Action<int>((int k) =>
               {
                   ++k;

                   return;
               })
            );

            tq.AddTask(zTaskRe);
            while (!zTaskRe.ResultValue.HasValue());
            var j = zTaskRe.ResultValue.Value;
            Console.WriteLine(j);

            TaskRegistryEntry yTaskRe = new TaskRegistryEntry(String.Format("task1"), arguments: new object[] { p, o },
            dlgt: new Func<int, int, int>((int k, int l) =>
            {

                return k + l;
            }));
            tq.AddTask(yTaskRe);
            while (!yTaskRe.ResultValue.HasValue());
            var h = yTaskRe.ResultValue.Value;
            Console.WriteLine((new object[] { p, o }).GetHashCode() == (new object[] { p, o }).GetHashCode());
            Console.ReadLine();
        }
    }
}
