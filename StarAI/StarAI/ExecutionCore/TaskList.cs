using StarAI.PathFindingCore;
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
        public static List<CustomTask> taskList = new List<CustomTask>();
        public static Task executioner = new Task(new Action(runTaskList));

        public static List<CustomTask> removalList = new List<CustomTask>();
        public static void runTaskList()
        {
           
            //myTask t = new myTask(StarAI.PathFindingCore.CropLogic.CropLogic.harvestSingleCrop);
             
            bool assignNewTask = true;

            while(ranAllTasks()==false)
            {


                //recalculate cost expenses every time a task runs because we don't know where we will be at any given moment. Kind of costly unfortunately but works.
            
                
                
            
            foreach (var task2 in taskList)
            {
                    if (removalList.Contains(task2)) continue;
                    object[] oArray = (object[])task2.objectParameterDataArray;
                   
                    try
                    {
                        TileNode t = (TileNode)oArray[0];
                        ModCore.CoreMonitor.Log("Premtive calculate 1");
                        task2.taskMetaData.calculateTaskCost(t, false);
                        object[] objArr = new object[3];
                        objArr[0] = (object)t;
                        objArr[1] = (object)task2.taskMetaData.path;
                        task2.objectParameterDataArray = objArr;
                    }
                    catch(Exception err)
                    {
                        List<TileNode> t = (List<TileNode>)oArray[0];
                        ModCore.CoreMonitor.Log("Premtive calculate 2");
                        foreach(var s in Utilities.tileExceptionList)
                        {
                            ModCore.CoreMonitor.Log(s.actionType);
                        }
                        task2.taskMetaData.calculateTaskCost(t, false);
                        object[] objArr = new object[3];
                        objArr[0] = (object)t; //List of trees to use for path calculations
                        objArr[1] = (object)task2.taskMetaData.path; //The path itself.
                        int malcolm = 0;
                        ModCore.CoreMonitor.Log("THIS IS MALCOLM:" + malcolm);
                        objArr[2] = (object)task2.taskMetaData.path.ElementAt(malcolm); //source of whatever is hit.
                        task2.objectParameterDataArray = objArr;
                        Utilities.tileExceptionList.Clear();
                    }
                  

                  
                    
                   
                //task.taskMetaData = new TaskMetaData(task.taskMetaData.name, PathFindingCore.Utilities.calculatePathCost(task.objectParameterDataArray), task.taskMetaData.staminaPrerequisite, task.taskMetaData.toolPrerequisite);
            }
                ModCore.CoreMonitor.Log("DONE CALCULATING JUNK NOW RUNNING TASK");
            //Some really cool delegate magic that sorts in place by the cost of the action!!!!
            taskList.Sort(delegate (CustomTask t1, CustomTask t2)
            {
                return t1.taskMetaData.cost.CompareTo(t2.taskMetaData.cost);
            });
                CustomTask v = taskList.ElementAt(0);
                int i = 0;
                while (removalList.Contains(v))
                {
                    v = taskList.ElementAt(i);
                    i++;
                }
                //  v.Start();

                if (v.taskMetaData.verifyAllPrerequisitesHit() == true)
                {
                    v.runTask();
                    Utilities.clearExceptionListWithName("Child");
                    Utilities.clearExceptionListWithName("Navigation");
                    removalList.Add(v);
                }
                else
                {
                    removalList.Add(v);
                }
            }


            taskList.Clear();
            removalList.Clear();
            
        }

        public static bool ranAllTasks()
        {
            foreach(CustomTask task in taskList)
            {
                if (removalList.Contains(task)) continue;
                else return false;
            }
            return true;
        }

        public static void printAllTaskMetaData()
        {
            ModCore.CoreMonitor.Log(taskList.Count.ToString());
            foreach (var task in taskList)
            {
                task.taskMetaData.printMetaData();
            }
        }
    }
}
