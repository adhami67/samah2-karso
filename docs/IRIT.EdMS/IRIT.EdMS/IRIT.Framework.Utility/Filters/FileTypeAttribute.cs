using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRIT.Framework.Utility.Filters
{
    public class FileTypeAttribute : ValidationAttribute
    {
        private readonly string[] _fileType;

        public FileTypeAttribute(string fileType)
        {
            _fileType = fileType.Split(',').Select(c => c.StartsWith(".") ? c.Remove(0,1).ToLower() : c.ToLower()).ToArray();
            // ".jpg", ".gif", ".png", ".pdf"
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
        
            return file != null && _fileType.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.') + 1));
        }
    }
}