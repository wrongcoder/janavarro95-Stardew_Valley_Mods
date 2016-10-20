using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StardewValley.PatchedUtilities
{
    class ObjectUtility
    {
       public static List<Object> object_list = new List<Object>();

        public static void getAllObjects()
        {
            if (object_list.Count > 0) return;
            Dictionary<int, string> my_dic = Game1.content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
            foreach (var key in my_dic.Keys)
            {
                object_list.Add(new Object(key, 1, false, -1, 0));

            }

        }

        public static List<Object> getAllObjectsAssociatedWithCategory(int category_number)
        {
            getAllObjects();
            List<Object> my_obj_list = new List<Object>();
            if (category_number > 0) return my_obj_list; ;
            foreach (var obj in object_list)
            {
                if (obj.category == category_number)
                {
                    my_obj_list.Add(obj);
                }
            }

                return my_obj_list;
        }


    }
}
