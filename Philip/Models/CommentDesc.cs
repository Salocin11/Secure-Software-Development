using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Philip.Models
{
    public class CommentDesc
    {
        public int CommentDescID { get; set; }

        [ForeignKey("Comment")]
        public int CommentID { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(32)]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        public string CommentName { get; set; }
        [System.ComponentModel.DataAnnotations.MaxLength(512)]
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        public string CommentWords { get; set; }
    }
}