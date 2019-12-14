using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.SaveAnywhere.Framework
{
    public class SaveAnywhereAPI
    {
        public SaveAnywhereAPI()
        {

        }

        public void addBeforeSaveEvent(string ID, Action BeforeSave)
        {
            SaveAnywhere.Instance.SaveManager.beforeCustomSavingBegins.Add(ID, BeforeSave);
        }
        public void addAfterSaveEvent(string ID, Action BeforeSave)
        {
            SaveAnywhere.Instance.SaveManager.afterCustomSavingCompleted.Add(ID, BeforeSave);
        }
        public void addAfterLoadEvent(string ID, Action BeforeSave)
        {
            SaveAnywhere.Instance.SaveManager.afterSaveLoaded.Add(ID, BeforeSave);
        }

    }
}
