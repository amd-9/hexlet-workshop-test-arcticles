using MicroArticles.Controllers;
using MicroArticles.Models;
using MicroArticles.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroArticles.Tests
{
    [TestFixture]
    public class MicroArticlesController_Should
    {
        private readonly IArticleFileProvider _fileProvider;
        private readonly IStorageProvider storageProvider;
        private readonly MicroArticlesContext _context;

        public MicroArticlesController_Should()
        {

            var fileProviderMock = new Mock<IArticleFileProvider>();
            var storageProviderMock = new Mock<IStorageProvider>();
        }

        [Test]
        public async Task ReturnFileNameIsEmpty()
        {
            var options = new DbContextOptionsBuilder<MicroArticlesContext>()
               .UseInMemoryDatabase(databaseName: "FileNameEmpty")
               .Options;

            using (var context = new MicroArticlesContext(options))
            {
                var controller = new MicroArticlesController(context, _fileProvider);

                var result = await controller.Download(string.Empty);
                var contentResult = result as ContentResult;

                Assert.AreEqual("Filename is empty", contentResult.Content);
            }           
        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithListOfMicroArticles()
        {
            var options = new DbContextOptionsBuilder<MicroArticlesContext>()
                .UseInMemoryDatabase(databaseName: "Index_Returns_List")
                .Options;

            using (var context = new MicroArticlesContext(options))
            {
                context.MicroArticle.Add(
                    new MicroArticle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Mushrooms",
                        Body = "Tasty",
                        ImageAddress = "http://localhost/image1.jpg"
                    }
                 );
                context.MicroArticle.Add(
                    new MicroArticle
                        {
                            Id = Guid.NewGuid(),
                            Name = "Donats",
                            Body = "Tasty",
                            ImageAddress = "http://localhost/image1.jpg"
                        }
                     );

                context.SaveChanges();

                var controller = new MicroArticlesController(context, _fileProvider);
                var result = await controller.Index();
                
                Assert.IsInstanceOf<ViewResult>(result);
                var viewResult = result as ViewResult;

                Assert.IsAssignableFrom<List<MicroArticle>>(viewResult.ViewData.Model);
                var microArticles = viewResult.ViewData.Model as List<MicroArticle>;

                Assert.AreEqual(2, microArticles.Count());
            }

        }
        
        [Test]
        public async Task Edit_ReturnsNotFoundWhenArcticleWithIdNotExists()
        {
            var options = new DbContextOptionsBuilder<MicroArticlesContext>()
                .UseInMemoryDatabase(databaseName: "Edit_Returns_NotFound")
                .Options;

            using (var context = new MicroArticlesContext(options))
            {
                context.MicroArticle.Add(
                    new MicroArticle
                    {
                        Id = Guid.NewGuid(),
                        Name = "Mushrooms",
                        Body = "Tasty",
                        ImageAddress = "http://localhost/image1.jpg"
                    }
                 );

                context.SaveChanges();

                var controller = new MicroArticlesController(context, _fileProvider);
                var result = await controller.Edit(Guid.NewGuid());

                Assert.IsInstanceOf<NotFoundResult>(result);
            }
        }
    }
}
