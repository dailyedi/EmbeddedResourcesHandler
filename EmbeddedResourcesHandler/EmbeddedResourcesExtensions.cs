using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmbeddedResourcesHandler
{
    public static class EmbeddedResourcesServicesExtensions
    {
        public static Func<string, string, bool> Contains = (x, y) => x.Contains(y),
            equals = (x, y) => string.Equals(x, y, StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// this function returns a stream from an embedded resource file name
        /// YOU SHOULD CLOSE THE STREAM WHEN YOU'RE DONE
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param> 
        /// <returns>a stream from the embedded file</returns>
        public static Stream GetFileStream(this Assembly assembly, string filename) 
            => assembly.GetFileStream(filename, EmbeddedResourcesServicesExtensions.Contains);


        /// <summary>
        /// this function returns a stream from an embedded resource file name
        /// YOU SHOULD CLOSE THE STREAM WHEN YOU'RE DONE
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>a stream from the embedded file</returns>
        public static Stream GetFileStream(this Assembly assembly, string filename,
            Func<string, string, bool> matchingPredicate)
        {
            var filePath = assembly.GetManifestResourceNames()
                .FirstOrDefault(x => matchingPredicate(x, filename));
            if (string.IsNullOrEmpty(filePath))
                throw new FileNotFoundException($"{filename} doesn't exist in the application embedded resources");
            return assembly.GetManifestResourceStream(filePath);
        }

        /// <summary>
        /// return a string content from an embedded resource file using the file name
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>the string content from the embedded file</returns>
        public static string GetFileString(this Assembly assembly, string filename)
        {
            using (var stream = assembly.GetFileStream(filename))
            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }

        /// <summary>
        /// return a string[] content from an embedded resource file using the file name
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>the string[] content from the embedded file</returns>
        public static string[] GetFileLines(this Assembly assembly, string filename)
        {
            using (var stream = assembly.GetFileStream(filename))
            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        /// <summary>
        /// return a string content from an embedded resource file using the file name
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>the string content from the embedded file</returns>
        public static async Task<string> GetFileStringAsync(this Assembly assembly, string filename)
        {
            using (var stream = assembly.GetFileStream(filename))
            using (var sr = new StreamReader(stream))
                return await sr.ReadToEndAsync();
        }

        /// <summary>
        /// return a string[] content from an embedded resource file using the file name
        /// </summary>
        /// <param name="filename">the file name of embedded resource</param>
        /// <returns>the string[] content from the embedded file</returns>
        public static async Task<string[]> GetFileLinesAsync(this Assembly assembly, string filename)
        {
            using (var stream = assembly.GetFileStream(filename))
            using (var sr = new StreamReader(stream))
                return (await sr.ReadToEndAsync()).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
    }
}
