using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarAI.ExecutionCore
{
    public class CustomTask
    {
        public delegate void ObjectTask(object obj);
        public delegate void VoidTask();


        public ObjectTask objectTask;
        public object objectParameterDataArray;
        public VoidTask voidTask;

        public TaskMetaData taskMetaData;
        public CustomTask(ObjectTask objTask,object[] arrayData, TaskMetaData TaskMetaData)
        {
            objectTask = objTask;
            objectParameterDataArray = arrayData;
            this.taskMetaData = TaskMetaData;
        }

        public CustomTask(VoidTask vTask, TaskMetaData TaskMetaData)
        {
            voidTask = vTask;
            this.taskMetaData = TaskMetaData;
        }

        public void runTask()
        {

            //Check Before running task if all prerequisites are working
            if (objectTask != null) objectTask.Invoke(objectParameterDataArray);

            if (voidTask != null) voidTask.Invoke();
        }

    }
}
