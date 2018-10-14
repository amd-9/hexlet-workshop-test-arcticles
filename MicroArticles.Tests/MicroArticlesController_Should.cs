using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using MicroArticles.Controllers;
using MicroArticles.Models;
using MicroArticles.Providers;
using MicroArticles.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroArticles.Tests
{
    [TestFixture]
    public class MicroArticlesController_Should
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        public MicroArticlesController_Should()
        {
            _factory = new MicroArticlesFactory<Startup>();
        }

        [SetUp]
        public void Init()
        {
            var fileProviderMock = new Mock<IArticleFileProvider>();
            fileProviderMock.Setup(provider => provider.SaveFileFromUriAsync(It.IsAny<string>()))
                .Returns(Task.FromResult("testFile"));

            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(serviceProvider => fileProviderMock.Object);
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Test]
        public async Task ReturnFileNameIsEmpty2()
        {
            var response = await _client.GetAsync("MicroArticles/Download");

            var content = await response.Content.ReadAsStringAsync();

            Assert.AreEqual("Filename is empty", content);

        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithListOfMicroArticles()
        {
            var indexPage = await _client.GetAsync("MicroArticles/Index");
            var content = await HtmlHelpers.GetDocumentAsync(indexPage);


            var aritcleElements = content.QuerySelectorAll("tr.microArticle");

            Assert.AreEqual(HttpStatusCode.OK, indexPage.StatusCode);
            Assert.AreEqual(2, aritcleElements.Count());
        }

        [Test]
        public async Task Edit_ReturnsNotFoundWhenArcticleWithIdNotExists()
        {
            var response = await _client.GetAsync($"MicroArticles/Edit/{Guid.NewGuid()}");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Test]
        public async Task CreateArticleAndRedirect()
        {
            var createPage = await _client.GetAsync("MicroArticles/Create");
            var antiForgeryToken = await AntiForgeryHelper.ExtractAntiForgeryToken(createPage);
            var content = await HtmlHelpers.GetDocumentAsync(createPage);

            var name = "Test";
            var body = "Test";
            var imageAdress = "http://localhost/image3.jpg";

            //CreateArticle
            var response = await _client.PostAsync("MicroArticles/Create", new FormUrlEncodedContent(
                new Dictionary<string, string> {
                    {"__RequestVerificationToken", antiForgeryToken},
                    { "Name", name },
                    { "Body", body },
                    { "ImageAddress", imageAdress }})
            );


            Assert.AreEqual(HttpStatusCode.OK, createPage.StatusCode);
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
            Assert.AreEqual("/", response.Headers.Location.OriginalString);
        }
    }
}
