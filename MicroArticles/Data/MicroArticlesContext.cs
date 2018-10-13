using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MicroArticles.Models
{
    public class MicroArticlesContext : DbContext
    {
        public MicroArticlesContext (DbContextOptions<MicroArticlesContext> options)
            : base(options)
        {
        }

        public DbSet<MicroArticles.Models.MicroArticle> MicroArticle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }
    }
}
