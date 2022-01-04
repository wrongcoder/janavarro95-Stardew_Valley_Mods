using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revitalize.Framework.Configs.ObjectsConfigs
{
    /// <summary>
    /// A config manager that specifically deals with loading config files for objects.
    /// </summary>
    public class ObjectConfigManager
    {
        public ObjectsConfig objectsConfig;

        public FeedThreasherConfig feedThreasherConfig;

        public ObjectConfigManager()
        {
            this.objectsConfig = ObjectsConfig.InitializeConfig();
            this.feedThreasherConfig = FeedThreasherConfig.InitializeConfig();
        }

    }
}
