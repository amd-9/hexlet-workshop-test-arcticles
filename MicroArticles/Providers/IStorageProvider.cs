using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroArticles.Providers
{
    public interface IStorageProvider
    {
        Task<byte[]> GetFileAsync(string fileName);
        Task<string> SaveFileAsync(byte[] fileContent);

    }
}
