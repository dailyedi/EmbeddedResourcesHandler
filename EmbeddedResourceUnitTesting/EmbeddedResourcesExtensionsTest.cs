using EmbeddedResourcesHandler;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace EmbeddedResourceUnitTesting
{
    public class EmbeddedResourcesExtensionsTest
    {
        private static Assembly _assembly;

        [SetUp]
        public void Setup()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }
        [Test]
        public void Contains_whenCalled_ReturnsTrue()
        {
            var result = EmbeddedResourcesServicesExtensions.Contains("HelloWorld.txt", "World");
            Assert.That(result, Is.True);
        }
        [Test]
        public void Equals_whenCalled_ReturnsTrue()
        {
            var result = EmbeddedResourcesServicesExtensions.equals("Read", "read");
            Assert.That(result, Is.True);
        }
        [Test]
        public void GetFileStream_FilenameNotExsitOrResourcesEmpty_ReturnException()
        {
             var list = _assembly.GetManifestResourceNames();
            Assert.That(() => EmbeddedResourcesServicesExtensions.GetFileStream(_assembly, "test")
            , Throws.Exception.TypeOf<FileNotFoundException>());
        }
        [Test]
        public void GetFileStream_FilenameExist_ReturnStream()
        {
            var result = EmbeddedResourcesServicesExtensions.GetFileStream(_assembly, "Resource");
            var tempResult = new StreamReader(result);
            var endResult = tempResult.ReadToEnd();
            Assert.That(endResult, Is.EqualTo("Hello\r\nWorld"));
        }
        [Test]
        public void GetFileString_WhenCalled_ReturnString()
        {
            var result = _assembly.GetFileString("Resource");
            Assert.That(result, Is.EqualTo("Hello\r\nWorld"));
        }
        [Test]
        public void GetFileLines_WhenCalled_ReturnString()
        {
            var result = _assembly.GetFileLines("Resource");
            Assert.That(result, Is.EquivalentTo(new string[] { "Hello", "World" }));
        }
        [Test]
        public async Task GetFileStringAsync_WhenCalled_ReturnString()
        {
            var result = await _assembly.GetFileStringAsync("Resource");
            Assert.That(result, Is.EqualTo("Hello\r\nWorld"));
        }
        [Test]
        public async Task GetFileLinesAsync_WhenCalled_ReturnString()
        {
            var result = await _assembly.GetFileLinesAsync("Resource");
            Assert.That(result, Is.EquivalentTo(new string[] { "Hello", "World" }));
        }
    }
}