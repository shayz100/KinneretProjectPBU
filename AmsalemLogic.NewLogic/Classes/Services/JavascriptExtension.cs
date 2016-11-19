using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace AmsalemLogic.NewLogic.Classes.Services
{
    public static class JavascriptExtension
    {
        public static string IncludeVersionedJs(string filename)
        {
            string version = GetVersion(filename);
            return filename + version;
        }

        public static string CreateVersionAndGetScriptTag(string fileName)
        {
            string js = IncludeVersionedJs(fileName);
            string result = "<script src='" + js + "' type='text/javascript'></script>";
            return result;
        }
        private static string GetVersion(string filename)
        {
            var model = MemoryCache.Default["SourceVersion_" + filename];
            var version
                = "";

            if (model == null)
            {
                var physicalPath = HttpContext.Current.Server.MapPath("~") + filename;
                var file = new System.IO.FileInfo(physicalPath);

                version = "?v=" + file.LastWriteTime.ToString("yyyyMMddhhmmss");
#if !DEBUG
                MemoryCache.Default["SourceVersion_" + filename] = version;
#endif
                return version;
            }
            else
            {
                version = model.ToString();
            }
            return version;
        }
    }
}
