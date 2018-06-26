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
            manager.BeforeSave += (sender, e) => { BeforeSave.Invoke(sender, e); };
            manager.AfterSave += (sender, e) => { AfterSave.Invoke(sender, e); };
            manager.AfterLoad += (sender, e) => { AfterLoad.Invoke(sender, e); };
        }  
    }
}
