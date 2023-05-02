using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.Revitalize.Framework.Utilities.JsonContentLoading
{
    /// <summary>
    /// Exception class used when an error occurs loading content for the mod.
    /// </summary>
    public class JsonContentLoadingException:Exception
    {

        public JsonContentLoadingException(string message) : base(message)
        {

        }
    }
}
