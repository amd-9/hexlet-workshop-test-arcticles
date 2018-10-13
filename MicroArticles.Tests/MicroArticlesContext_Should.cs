using MicroArticles.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroArticles.Tests
{
    [TestFixture]
    public class MicroArticlesContext_Should
    {
        [Test]
        public async Task AddArticleToDatabase()
        {
            var options = new DbContextOptionsBuilder<MicroArticlesContext>()
                .UseInMemoryDatabase(databaseName: "Add_article_to_DB")
                .Options;

            using (var context = new MicroArticlesContext(options))
            {
                context.MicroArticle.Add(
                    new MicroArticle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Hello",
                        Body = "World",
                        ImageAddress = "http://localhost/image.jpg"
                    }
                 );

                context.SaveChanges();
            }

            using (var context = new MicroArticlesContext(options))
            {
                var articles = await context.MicroArticle.ToListAsync();

                Assert.AreEqual(1, articles.Count);
            }
        }

        [Test]
        public async Task FindArticleInDatabase()
        {
            var options = new DbContextOptionsBuilder<MicroArticlesContext>()
                .UseInMemoryDatabase(databaseName: "Find_article_in_DB")
                .Options;

            var id = Guid.NewGuid();

            using (var context = new MicroArticlesContext(options))
            {
                

                context.MicroArticle.Add(
                    new MicroArticle
                    {
                        Id = id,
                        Name = "Hello",
                        Body = "World",
                        ImageAddress = "http://localhost/image.jpg"
                    }
                 );

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new MicroArticlesContext(options))
            {
                var articles = await context.MicroArticle.ToListAsync();
                var result = articles.FindAll(x => x.Id == id);

                Assert.AreEqual(1, result.Count);
            }
        }
    }
}
