using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmbeddedResourcesHandler
{
    public class EmbeddedResourcesServices
    {
        private  Assembly asm;
        public static Func<string, string, bool> Contains = (x, y) => x.Contains(y),
            equals = (x, y) => string.Equals(x, y, StringComparison.CurrentCultureIgnoreCase);

        public EmbeddedResourcesServices(Assembly calleeAsm = null) => 
            asm = calleeAsm ?? Assembly.GetCallingAssembly();

        public EmbeddedResourcesServices(string path) => asm = Assembly.LoadFile(path);


        /// <summary>
        /// this function returns a stream from an embedded resource file name
        /// YOU SHOULD CLOSE THE STREAM WHEN YOU'RE DONE
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>a stream from the embedded file</returns>
        public Stream GetFileStream(string filename) => GetFileStream(filename, Contains);

        /// <summary>
        /// this function returns a stream from an embedded resource file name
        /// YOU SHOULD CLOSE THE STREAM WHEN YOU'RE DONE
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <param name="matchingPredicate">the predicate to match resources with the file name on</param>
        /// <returns>a stream from the embedded file</returns>
        public Stream GetFileStream(string filename, Func<string, string, bool> matchingPredicate)
        {
            var filePath = asm.GetManifestResourceNames()
                .FirstOrDefault(x => matchingPredicate(x, filename));
            if (string.IsNullOrEmpty(filePath))
                throw new FileNotFoundException($"{filename} doesn't exist in the application embedded resources");
            return asm.GetManifestResourceStream(filePath);
        }

        /// <summary>
        /// return a string content from an embedded resource file using the file name
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>the string content from the embedded file</returns>
        public string GetFileString(string filename)
        {
            using (var stream = GetFileStream(filename))
            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }

        /// <summary>
        /// return a string[] content from an embedded resource file using the file name
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>the string[] content from the embedded file</returns>
        public string[] GetFileLines(string filename)
        {
            using (var stream = GetFileStream(filename))
            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        /// <summary>
        /// return a string content from an embedded resource file using the file name
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>the string content from the embedded file</returns>
        public async Task<string> GetFileStringAsync(string filename)
        {
            using (var stream = GetFileStream(filename))
            using (var sr = new StreamReader(stream))
                return await sr.ReadToEndAsync();
        }

        /// <summary>
        /// return a string[] content from an embedded resource file using the file name
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>the string[] content from the embedded file</returns>
        public async Task<string[]> GetFileLinesAsync(string filename)
        {
            using (var stream = GetFileStream(filename))
            using (var sr = new StreamReader(stream))
                return (await sr.ReadToEndAsync()).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
