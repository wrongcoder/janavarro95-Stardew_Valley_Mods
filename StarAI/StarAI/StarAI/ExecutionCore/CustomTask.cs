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
        public CustomTask(ObjectTask objTask,object[] arrayData)
        {
            objectTask = objTask;
            objectParameterDataArray = arrayData;
        }

        public CustomTask(VoidTask vTask)
        {
            voidTask = vTask;
        }

        public void runTask()
        {
            if (objectTask != null) objectTask.Invoke(objectParameterDataArray);

            if (voidTask != null) voidTask.Invoke();
        }

    }
}
