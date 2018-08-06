using System;
using Omegasis.SaveAnywhere.Framework;

namespace Omegasis.SaveAnywhere.API
{
    public class SaveAnywhereAPI : ISaveAnywhereAPI
    {
        public event EventHandler BeforeSave;
        public event EventHandler AfterSave;
        public event EventHandler AfterLoad;


        public SaveAnywhereAPI(SaveManager manager)
        {
            BeforeSave = new EventHandler(empty);
            AfterSave= new EventHandler(empty);
            AfterLoad= new EventHandler(empty);
            manager.BeforeSave += (sender, e) => { BeforeSave.Invoke(sender, e); };
            manager.AfterSave += (sender, e) => { AfterSave.Invoke(sender, e); };
            manager.AfterLoad += (sender, e) => { AfterLoad.Invoke(sender, e); };
        }

        /// <summary>
        /// Used to initialize empty event handlers.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        private void empty(object o, EventArgs args){

        }
    }
}
