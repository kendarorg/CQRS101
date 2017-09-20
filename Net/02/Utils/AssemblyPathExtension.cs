using System.IO;
using System.Reflection;

namespace System
{
    public static class AssemblyPathExtension
    {
        public static String GetAssemblyPath(this Assembly asm)
        {
            var codeBaseAbsolutePathUri = new Uri(asm.GetName().CodeBase).AbsolutePath;
            codeBaseAbsolutePathUri = codeBaseAbsolutePathUri.Replace("%20", " ");
            return Path.GetDirectoryName(codeBaseAbsolutePathUri);
        }

        public static String AssemblyDir
        {
            get { return Assembly.GetExecutingAssembly().GetAssemblyPath(); }
        }
    }
}
