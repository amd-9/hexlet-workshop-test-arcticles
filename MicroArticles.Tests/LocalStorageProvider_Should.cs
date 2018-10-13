using MicroArticles.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using System.Threading.Tasks;

namespace MicroArticles.Tests
{
    [TestFixture]
    public class LocalStorageProvider_Should
    {
        public readonly IStorageProvider _storageProvider;
        public LocalStorageProvider_Should()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\temp\UploadFiles\image.jpg", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
            });
            
            fileSystem.Directory.SetCurrentDirectory(@"c:\temp\");

            _storageProvider = new LocalStorageProvider(fileSystem);            
        }

        [Test]
        public async Task GetFileByName()
        {
            var expectContent = new byte[] { 0x12, 0x34, 0x56, 0xd2 };        
            var fileContent = await _storageProvider.GetFileAsync("image.jpg");

            Assert.That(expectContent, Is.EquivalentTo(fileContent));
        }

        [Test]
        public async Task SaveFile ()
        {
            var fileContent = new byte[] { 0x12, 0x34, 0x56, 0xd2 };

            var savedFileName = await _storageProvider.SaveFileAsync(fileContent);
            var savedFileContent = await _storageProvider.GetFileAsync(savedFileName);

            Assert.That(fileContent, Is.EquivalentTo(savedFileContent));
        }
    }
}
