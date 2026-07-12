using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRIT.Framework.Utility.Utility
{
    public static class FileManager
    {
        public static byte[] ConvertToByteArray(this HttpPostedFileBase fileInput)
        {
            var target = new MemoryStream();
            fileInput.InputStream.CopyTo(target);
            return target.ToArray();
        }
    }
}
