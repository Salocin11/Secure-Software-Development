using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Philip.Models
{
    public class FinalComment
    {
        [Key]
        public int CommentID { get; set; }

        [ForeignKey("Article")]
        public int ArticleID { get; set; }

        [Required(ErrorMessage = "This is a required field")]
        [StringLength(32, MinimumLength = 1)]
        public string CommentName { get; set; }

        [Required(ErrorMessage = "This is a required field")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "The comment is limited to 3 and 256 letters.")]
        public string CommentWords { get; set; }
    }
}