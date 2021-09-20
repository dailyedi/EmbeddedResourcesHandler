using EmbeddedResourcesHandler;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedResourceUnitTesting
{
    class EmbeddedResourcesServicesTests
    {
        public EmbeddedResourcesServices _embeddedResource;
        public EmbeddedResourcesServices _embeddedResourceExternal;
        [SetUp]
        public void Setup()
        {
            _embeddedResource = new EmbeddedResourcesServices();
            _embeddedResourceExternal = new EmbeddedResourcesServices(@"C:\Users\Mohamed_Reda\source\repos\EmbeddedResourcesHandler\EmbeddedResourceUnitTesting\MockingFilesTesting\TestResourceAssembly.dll");
        }
        [Test]
        public void Contains_whenCalled_ReturnsTrue()
        {
            var result = EmbeddedResourcesServices.Contains("HelloWorld.txt", "World");
            Assert.That(result, Is.True);
        }
        [Test]
        public void Equals_whenCalled_ReturnsTrue()
        {
            var result = EmbeddedResourcesServices.equals("Read", "read");
            Assert.That(result, Is.True);
        }
        [Test]
        public void GetFileStream_FilenameNotExsitOrResourcesEmpty_ReturnException()
        {
            Assert.That(() => _embeddedResource.GetFileStream("test")
            , Throws.Exception.TypeOf<FileNotFoundException>());
        }
        [Test]
        public void GetFileStream_FilenameNotExsitOrResourcesEmptyExternal_ReturnException()
        {
            Assert.That(() => _embeddedResourceExternal.GetFileStream("test")
            , Throws.Exception.TypeOf<FileNotFoundException>());
        }
        [Test]
        public void GetFileStream_FilenameExist_ReturnStream()
        {
            var result = _embeddedResource.GetFileStream("Resource");
            var tempResult = new StreamReader(result);
            var endResult = tempResult.ReadToEnd();
            Assert.That(endResult, Is.EqualTo("Hello\r\nWorld"));
        }
        [Test]
        public void GetFileStream_FilenameExistExternal_ReturnStream()
        {
            var result = _embeddedResourceExternal.GetFileStream("ResourceExternal");
            var tempResult = new StreamReader(result);
            var endResult = tempResult.ReadToEnd();
            Assert.That(endResult, Is.EqualTo("Daily\r\nEdi"));
        }
        [Test]
        public void GetFileString_WhenCalled_ReturnString()
        {
            var result = _embeddedResource.GetFileString("Resource");
            Assert.That(result, Is.EqualTo("Hello\r\nWorld"));
        }
        [Test]
        public void GetFileString_WhenCalledExternal_ReturnString()
        {
            var result = _embeddedResourceExternal.GetFileString("ResourceExternal");
            Assert.That(result, Is.EqualTo("Daily\r\nEdi"));
        }

        [Test]
        public void GetFileLines_WhenCalled_ReturnString()
        {
            var result = _embeddedResource.GetFileLines("Resource");
            Assert.That(result, Is.EquivalentTo(new string[] { "Hello", "World" }));
        }
        [Test]
        public void GetFileLines_WhenCalledExternal_ReturnString()
        {
            var result = _embeddedResourceExternal.GetFileLines("ResourceExternal");
            Assert.That(result, Is.EquivalentTo(new string[] { "Daily", "Edi" }));
        }
        [Test]
        public async Task GetFileStringAsync_WhenCalled_ReturnString()
        {
            var result = await _embeddedResource.GetFileStringAsync("Resource");
            Assert.That(result, Is.EqualTo("Hello\r\nWorld"));
        }
        [Test]
        public async Task GetFileStringAsync_WhenCalledExternal_ReturnString()
        {
            var result = await _embeddedResourceExternal.GetFileStringAsync("ResourceExternal");
            Assert.That(result, Is.EqualTo("Daily\r\nEdi"));
        }
        [Test]
        public async Task GetFileLinesAsync_WhenCalled_ReturnString()
        {
            var result = await _embeddedResource.GetFileLinesAsync("Resource");
            Assert.That(result, Is.EquivalentTo(new string[] { "Hello", "World" }));
        }
        [Test]
        public async Task GetFileLinesAsync_WhenCalledExternal_ReturnString()
        {
            var result = await _embeddedResourceExternal.GetFileLinesAsync("ResourceExternal");
            Assert.That(result, Is.EquivalentTo(new string[] { "Daily", "Edi" }));
        }
    }
}
