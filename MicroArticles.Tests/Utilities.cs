using MicroArticles.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroArticles.Tests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(MicroArticlesContext context)
        {
            context.MicroArticle.Add(new MicroArticle
                 {
                     Id = Guid.NewGuid(),
                     Name = "Mushrooms",
                     Body = "Tasty",
                     ImageAddress = "http://localhost/image1.jpg"
                 });
            context.MicroArticle.Add(new MicroArticle
                {
                    Id = Guid.NewGuid(),
                    Name = "Donats",
                    Body = "Tasty",
                    ImageAddress = "http://localhost/image2.jpg"
                });

            context.SaveChanges();
        }
    }
}
