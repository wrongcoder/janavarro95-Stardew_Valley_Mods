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
        public static Task executioner = new Task(new Action(runTaskList));

        public static void runTaskList()
        {

            List<Task> removalList = new List<Task>();
            bool assignNewTask = true;
            for (int i= 0; i < taskList.Count;i++)
            {
                ModCore.CoreMonitor.Log("I value: " + i);
                ModCore.CoreMonitor.Log("Count: " + taskList.Count);
                ModCore.CoreMonitor.Log("GAOL AMOUNT: " + PathFindingCore.PathFindingLogic.goals.Count);
                Task v = taskList[i];
                //  v.Start();

                if (assignNewTask)
                {
                    //ModCore.CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    ModCore.fun = v;
                    ModCore.fun.Start();
                    assignNewTask = false;
                    //i--;
                    continue;
                }

                if (ModCore.fun.Status == TaskStatus.Running)
                {
                    assignNewTask = false;
                    //ModCore.CoreMonitor.Log("TASK IS RUNNING CAN'T PATHFIND AT THE MOMENT", LogLevel.Alert);
                    i--;
                    continue;
                    //return;
                }
                if (ModCore.fun.Status == TaskStatus.RanToCompletion)
                {

                    ModCore.fun = v;
                    ModCore.fun.Start();
                    assignNewTask = false;
                    //i--;
                    removalList.Add(v);
                    continue;
                    // return;
                }

                if (ModCore.fun.Status == TaskStatus.Faulted)
                {
                    ModCore.fun = v;
                    ModCore.fun.Start();
                    assignNewTask = false;
                    //i--;
                    removalList.Add(v);
                    continue;
                    //ModCore.CoreMonitor.Log(ModCore.fun.Exception.ToString());
                    //ModCore.CoreMonitor.Log("CREATE AND RUN A TASK!!! PATHFINDING!");
                    //ModCore.fun = v;
                    //ModCore.fun.Start();
                }

               
            }
            foreach(var v in removalList)
            {
                taskList.Remove(v);
            }

            
        }
    }
}
