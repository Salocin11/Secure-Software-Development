using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Philip.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        [ForeignKey("Article")]
        public int ArticleID { get; set; }
    }
}
