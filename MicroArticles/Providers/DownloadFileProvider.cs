using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroArticles.Providers
{
    public class DownloadFileProvider : IArticleFileProvider
    {
        private readonly IStorageProvider _storageProvider;
        public DownloadFileProvider(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }
        public async Task<string> SaveFileFromUriAsync(string fileUri)
        {
            using (var client = new HttpClient())
            {

                using (var result = await client.GetAsync(fileUri))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var fileContent = await result.Content.ReadAsByteArrayAsync();
                        return await _storageProvider.SaveFileAsync(fileContent);
                    }

                }
            }
            return null;
        }

        public async Task<byte[]> GetFileByNameAsync(string fileName)
        {
            return await _storageProvider.GetFileAsync(fileName);
        }
    }
}
