using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace MicroArticles.Providers
{
    public class LocalStorageProvider : IStorageProvider
    {
        private readonly string  _storagePath;
        private readonly IFileSystem _fileSystem;
        public LocalStorageProvider(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _storagePath = fileSystem.Path.Combine(_fileSystem.Directory.GetCurrentDirectory(), "UploadFiles");

            if (!_fileSystem.Directory.Exists(_storagePath))
                _fileSystem.Directory.CreateDirectory(_storagePath);
        }
        public async Task<byte[]> GetFileAsync(string fileName)
        {
            var filePath = _fileSystem.Path.Combine(_storagePath, fileName);
            if (!_fileSystem.File.Exists(filePath))
                return null;

            return await Task.Run(() => _fileSystem.File.ReadAllBytes(filePath));

        }

        public async Task<string> SaveFileAsync(byte[] fileContent)
        {
            var fileName = $"upload_{Guid.NewGuid()}";
            await Task.Run(() =>_fileSystem.File.WriteAllBytes(_fileSystem.Path.Combine(_storagePath, fileName), fileContent));
            
            return fileName;
        }
    }
}
