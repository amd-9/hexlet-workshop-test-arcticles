using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroArticles.Providers
{
    public interface IArticleFileProvider
    {                
        Task<string> SaveFileFromUriAsync(string fileUri);
        Task<byte[]> GetFileByNameAsync(string fileName);
    }
}
