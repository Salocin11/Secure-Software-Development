using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Philip.Models
{
    public class Article
    {
        private static int instances = 0;
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }
        [StringLength(60, MinimumLength = 1)]
        [Required]
        public string Author { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Content { get; set; } // Database should be using the Blob data type
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Article()
        {
            instances++;
        }
        ~Article()
        {
            instances--;
        }

        public static int GetArticleCount()
        {
            return instances;
        }
    }
}
