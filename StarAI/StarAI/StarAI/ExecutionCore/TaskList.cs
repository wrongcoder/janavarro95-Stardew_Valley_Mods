using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarAI.ExecutionCore
{
    class TaskList
    {
        public static List<Task> taskList = new List<Task>();


        public static void runTaskList()
        {

            List<Task> removalList = new List<Task>();
            bool assignNewTask = true;
            for (int i= 0; i <= taskList.Count;i++)
            {
                Task v = taskList[i];
                //  v.Start();

                if (assignNewTask)
                {
                    ModCore.CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    ModCore.fun = v;
                    ModCore.fun.Start();
                    assignNewTask = false;
                }

                if (ModCore.fun.Status == TaskStatus.Running)
                {
                    assignNewTask = false;
                    ModCore.CoreMonitor.Log("TASK IS RUNNING CAN'T PATHFIND AT THE MOMENT", LogLevel.Alert);
                    i--;
                    continue;
                    //return;
                }
                if (ModCore.fun.Status == TaskStatus.RanToCompletion)
                {

                    assignNewTask = true;
                    continue;
                    // return;
                }

                if (ModCore.fun.Status == TaskStatus.Faulted)
                {
                    assignNewTask = true;
                    continue;
                    //ModCore.CoreMonitor.Log(ModCore.fun.Exception.ToString());
                    //ModCore.CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    //ModCore.fun = v;
                    //ModCore.fun.Start();
                }

                removalList.Add(v);
            }
            foreach(var v in removalList)
            {
                taskList.Remove(v);
            }
            removalList.Clear();

        }
    }
}
