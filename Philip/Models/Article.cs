using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Philip.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Content { get; set; } // Database should be using the Blob data type
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
