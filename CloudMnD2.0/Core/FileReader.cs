using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CloudMnD2._0.Core
{
    public static class FileReader
    {
        public static string ReadText(string fileName)
        {
            var output = string.Empty;

            try
            {
                var dir = System.IO.Directory.GetCurrentDirectory();
                var path = Path.Combine(dir, fileName);
                output = System.IO.File.ReadAllText(path);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
            }

            return output;
        }
        public static string ReadTextFromPath(string path, string fileName)
        {
            return System.IO.File.ReadAllText(CombinePaths(path, fileName));
        }
        private static string CombinePaths(string p1, string p2)
        {
            try
            {
                return Path.Combine(p1, p2);
            }
            catch (System.Exception e)
            {
                if (p1 == null)
                    p1 = "null";
                if (p2 == null)
                    p2 = "null";
                Console.WriteLine("You cannot combine '{0}' and '{1}' because: {2}{3}",
                    p1, p2, Environment.NewLine, e.Message);
            }
            return string.Empty;
        }
    }
}
