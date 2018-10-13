using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroArticles.Models
{
    public class MicroArticle
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Body { get; set; }
        [Url]
        public string ImageAddress { get; set; }
        public string ImageFileName { get; set; }
    }
}
